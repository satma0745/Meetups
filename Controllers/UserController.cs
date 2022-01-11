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
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("/api/users")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class UserController : ControllerBase
{
    private readonly ApplicationContext context;
    private readonly IMapper mapper;

    private readonly SecurityKey signingKey;
    private readonly TimeSpan accessTokenLifetime;
    private readonly JwtSecurityTokenHandler tokenHandler;

    public UserController(ApplicationContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;

        // This key is used to sign tokens so that no one can tamper with them.
        // TODO: replace with easily configurable solution
        var rawSigningKey = "cock_sucker_1337";
        var signingKeyBytes = Encoding.ASCII.GetBytes(rawSigningKey);
        signingKey = new SymmetricSecurityKey(signingKeyBytes);
        
        // TODO: replace with easily configurable solution
        accessTokenLifetime = TimeSpan.FromMinutes(5);

        // Fixes JWT Claims names (by default Microsoft replaces them with links leading to nowhere) 
        tokenHandler = new JwtSecurityTokenHandler();
        tokenHandler.InboundClaimTypeMap.Clear();
        tokenHandler.OutboundClaimTypeMap.Clear();
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
        if (accessToken is null)
        {
            return Unauthorized();
        }
        
        IDictionary<string, string> claims;
        // This piece of shit throw an exception if token is invalid
        try
        {
            var validationParameters = new TokenValidationParameters
            {
                RequireSignedTokens = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                
                ValidateAudience = false,
                ValidateIssuer = false,
                
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            
            // Never mind the "out _" part. It's just a bad library design from Microsoft
            var claimsInfo = tokenHandler.ValidateToken(accessToken, validationParameters, out _);
            claims = claimsInfo.Claims.ToDictionary(claim => claim.Type, claim => claim.Value);
        }
        catch (Exception)
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

        var accessTokenDescriptor = new SecurityTokenDescriptor
        {
            Claims = new Dictionary<string, object>
            {
                {"sub", user.Id} // token owner id
            },
            Expires = DateTime.UtcNow.Add(accessTokenLifetime), // token is considered invalid after this point in time
            SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha512)
        };
        var accessToken = tokenHandler.CreateToken(accessTokenDescriptor);
        var encodedAccessToken = tokenHandler.WriteToken(accessToken);

        return Ok(encodedAccessToken);
    }
}
