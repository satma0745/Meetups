namespace Meetups.Controllers;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BCrypt.Net;
using Meetups.Context;
using Meetups.DataTransferObjects.User;
using Meetups.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("/api/users")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class UserController : ControllerBase
{
    private readonly ApplicationContext context;
    private readonly IMapper mapper;

    private readonly SigningCredentials signingCredentials;
    private readonly JwtSecurityTokenHandler tokenHandler;
    
    private readonly TimeSpan accessTokenLifetime;
    private readonly TimeSpan refreshTokenLifetime;

    public UserController(ApplicationContext context, IMapper mapper, IConfiguration configuration)
    {
        this.context = context;
        this.mapper = mapper;

        // This key is used to sign tokens so that no one can tamper with them.
        var rawSigningKey = configuration["Auth:SecretKey"];
        var signingKeyBytes = Encoding.ASCII.GetBytes(rawSigningKey);
        var signingKey = new SymmetricSecurityKey(signingKeyBytes);
        signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha512);

        // Fixes JWT Claims names (by default Microsoft replaces them with links leading to nowhere) 
        tokenHandler = new JwtSecurityTokenHandler();
        tokenHandler.InboundClaimTypeMap.Clear();
        tokenHandler.OutboundClaimTypeMap.Clear();
        
        var accessTokenLifetimeInMinutes = int.Parse(configuration["Auth:AccessTokenLifetimeInMinutes"]);
        accessTokenLifetime = TimeSpan.FromMinutes(accessTokenLifetimeInMinutes);

        var refreshTokenLifetimeInDays = int.Parse(configuration["Auth:RefreshTokenLifetimeInDays"]);
        refreshTokenLifetime = TimeSpan.FromDays(refreshTokenLifetimeInDays);
    }

    /// <summary>Get specific user (with the specified id).</summary>
    /// <param name="id">ID of the user of interest.</param>
    /// <response code="200">User was retrieved successfully.</response>
    /// <response code="404">User with the specified id does not exist.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ReadUserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSpecificUser([FromRoute] Guid id)
    {
        var user = await context.Users.SingleOrDefaultAsync(user => user.Id == id);
        if (user is null)
        {
            return NotFound();
        }

        var readDto = mapper.Map<ReadUserDto>(user);
        return Ok(readDto);
    }

    /// <summary>Get specific user (with the specified username).</summary>
    /// <param name="username">Username of the user of interest.</param>
    /// <response code="200">User was retrieved successfully.</response>
    /// <response code="404">User with the specified username does not exist.</response>
    [HttpGet("{username}")]
    [ProducesResponseType(typeof(ReadUserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSpecificUser([FromRoute] string username)
    {
        var user = await context.Users.SingleOrDefaultAsync(user => user.Username == username);
        if (user is null)
        {
            return NotFound();
        }

        var readDto = mapper.Map<ReadUserDto>(user);
        return Ok(readDto);
    }

    [HttpGet("who-am-i")]
    public async Task<IActionResult> GetCurrentUserInfo([FromQuery] string accessToken)
    {
        if (!TryParseToken(accessToken, out var claims))
        {
            return Unauthorized();
        }
        var currentUserId = Guid.Parse(claims["sub"]);

        var user = await context.Users.SingleOrDefaultAsync(user => user.Id == currentUserId);
        if (user is null)
        {
            return Unauthorized();
        }

        var readDto = mapper.Map<ReadUserDto>(user);
        return Ok(readDto);
    }

    /// <summary>Register a new user.</summary>
    /// <param name="registerDto">DTO to create user from.</param>
    /// <response code="200">New user was created successfully.</response>
    /// <response code="400">Validation failed for DTO.</response>
    /// <response code="409">User with the same username already exists.</response>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterNewUser([FromBody] RegisterUserDto registerDto)
    {
        var usernameTaken = await context.Users.AnyAsync(user => user.Username == registerDto.Username);
        if (usernameTaken)
        {
            return Conflict();
        }
        
        var user = mapper.Map<User>(registerDto);
        user.Id = Guid.NewGuid();
        user.Password = BCrypt.HashPassword(registerDto.Password);

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("authenticate")]
    public async Task<IActionResult> AuthenticateUser([FromBody] AuthenticateUserDto authenticateDto)
    {
        var user = await context.Users.SingleOrDefaultAsync(user => user.Username == authenticateDto.Username);
        if (user is null)
        {
            return NotFound();
        }

        var passwordMatches = BCrypt.Verify(authenticateDto.Password, user.Password);
        if (!passwordMatches)
        {
            return Conflict();
        }

        // Persist refresh token so it can be used later
        var refreshToken = new RefreshToken
        {
            TokenId = Guid.NewGuid(),
            UserId = user.Id
        };
        context.RefreshTokens.Add(refreshToken);
        await context.SaveChangesAsync();

        var tokenPair = IssueTokenPair(user.Id, refreshToken.TokenId);
        return Ok(tokenPair);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshTokenPair([FromBody] string refreshToken)
    {
        if (!TryParseToken(refreshToken, out var claims))
        {
            return BadRequest();
        }
        var currentUserId = Guid.Parse(claims["sub"]);
        var refreshTokenId = Guid.Parse(claims["jti"]);

        var userExists = await context.Users.AnyAsync(user => user.Id == currentUserId);
        var oldRefreshToken = await context.RefreshTokens
            .SingleOrDefaultAsync(token => token.TokenId == refreshTokenId);
        if (!userExists || oldRefreshToken is null)
        {
            // Cannot issue token for user that does not even exist (was deleted)
            // Cannot use refresh token that is not persisted to a db (fake or used)
            return BadRequest();
        }

        // Replace old refresh token with the new one
        var newRefreshToken = new RefreshToken
        {
            UserId = currentUserId,
            TokenId = Guid.NewGuid()
        };
        context.RefreshTokens.Remove(oldRefreshToken);
        context.RefreshTokens.Add(newRefreshToken);
        await context.SaveChangesAsync();

        var tokenPair = IssueTokenPair(currentUserId, newRefreshToken.TokenId);
        return Ok(tokenPair);
    }

    private bool TryParseToken(string token, out IDictionary<string, string> payload)
    {
        try
        {
            var validationParameters = new TokenValidationParameters
            {
                RequireSignedTokens = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingCredentials.Key,
                
                ValidateAudience = false,
                ValidateIssuer = false,
                
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            
            var claimsInfo = tokenHandler.ValidateToken(token, validationParameters, out _);
            payload = claimsInfo.Claims.ToDictionary(claim => claim.Type, claim => claim.Value);

            return true;
        }
        catch (Exception)
        {
            payload = null;
            return false;
        }
    }

    private TokenPairDto IssueTokenPair(Guid userId, Guid refreshTokenId)
    {
        var accessToken = IssueToken(new Dictionary<string, object> {{"sub", userId}}, accessTokenLifetime);
        var refreshToken = IssueToken(new Dictionary<string, object>
        {
            {"sub", userId},
            {"jti", refreshTokenId}
        }, refreshTokenLifetime);

        return new TokenPairDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    private string IssueToken(IDictionary<string, object> payload, TimeSpan lifetime)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Claims = payload,
            Expires = DateTime.UtcNow.Add(lifetime),
            SigningCredentials = signingCredentials
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
