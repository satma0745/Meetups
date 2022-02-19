namespace Meetups.Backend.Application.Features.Auth.AuthenticateUser;

using System;
using System.Threading.Tasks;
using AutoMapper;
using BCrypt.Net;
using Meetup.Contract.Models.Features.Auth.AuthenticateUser;
using Meetup.Contract.Models.Primitives;
using Meetups.Backend.Application.Seedwork;
using Meetups.Backend.Persistence.Context;
using Meetups.Backend.Persistence.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Tags("Auth")]
public class Controller : ApiControllerBase
{
    private readonly TokenHelper tokenHelper;

    public Controller(ApplicationContext context, IMapper mapper, TokenHelper tokenHelper)
        : base(context, mapper) =>
        this.tokenHelper = tokenHelper;
    
    /// <summary>Issue token pair for provided user credentials.</summary>
    /// <param name="request">User credentials.</param>
    /// <response code="200">User authenticated successfully.</response>
    /// <response code="404">User with specified username does not exist.</response>
    /// <response code="409">Incorrect password provided.</response>
    [HttpPost("auth/authenticate")]
    [ProducesResponseType(typeof(TokenPairDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AuthenticateUser([FromBody] RequestDto request)
    {
        var user = await Context.Users.SingleOrDefaultAsync(user => user.Username == request.Username);
        if (user is null)
        {
            return NotFound();
        }

        var passwordMatches = BCrypt.Verify(request.Password, user.Password);
        if (!passwordMatches)
        {
            return Conflict();
        }

        // Persist refresh token so it can be used later
        var persistedRefreshToken = new RefreshToken
        {
            TokenId = Guid.NewGuid(),
            UserId = user.Id
        };
        Context.RefreshTokens.Add(persistedRefreshToken);
        await Context.SaveChangesAsync();

        var (accessToken, refreshToken) = tokenHelper.IssueTokenPair(user, persistedRefreshToken.TokenId);
        var tokenPairDto = new TokenPairDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
        return Ok(tokenPairDto);
    }
}