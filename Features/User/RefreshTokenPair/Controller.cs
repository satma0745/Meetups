﻿namespace Meetups.Features.User.RefreshTokenPair;

using System;
using System.Threading.Tasks;
using AutoMapper;
using Meetups.Features.Shared;
using Meetups.Persistence.Context;
using Meetups.Persistence.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Tags("User")]
public class Controller : ApiControllerBase
{
    private readonly TokenHelper tokenHelper;

    public Controller(ApplicationContext context, IMapper mapper, TokenHelper tokenHelper)
        : base(context, mapper) =>
        this.tokenHelper = tokenHelper;
    
    /// <summary>Re-issue token pair using refresh token.</summary>
    /// <param name="oldRefreshToken">Refresh token.</param>
    /// <response code="200">Token pair was successfully re-issued.</response>
    /// <response code="400">Fake, damaged, expired or used refresh token was provided.</response>
    [HttpPost("users/refresh")]
    [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RefreshTokenPair([FromBody] string oldRefreshToken)
    {
        if (!tokenHelper.TryParseToken(oldRefreshToken, out var claims))
        {
            return BadRequest();
        }
        var currentUserId = Guid.Parse(claims["sub"]);
        var refreshTokenId = Guid.Parse(claims["jti"]);

        var userExists = await Context.Users.AnyAsync(user => user.Id == currentUserId);
        var oldPersistedRefreshToken = await Context.RefreshTokens
            .SingleOrDefaultAsync(token => token.TokenId == refreshTokenId);
        if (!userExists || oldPersistedRefreshToken is null)
        {
            // Cannot issue token for user that does not even exist (was deleted)
            // Cannot use refresh token that is not persisted to a db (fake or used)
            return BadRequest();
        }

        // Replace old refresh token with the new one
        var newPersistedRefreshToken = new RefreshToken
        {
            UserId = currentUserId,
            TokenId = Guid.NewGuid()
        };
        Context.RefreshTokens.Remove(oldPersistedRefreshToken);
        Context.RefreshTokens.Add(newPersistedRefreshToken);
        await Context.SaveChangesAsync();

        var newRefreshTokenId = newPersistedRefreshToken.TokenId;
        var (accessToken, newRefreshToken) = tokenHelper.IssueTokenPair(currentUserId, newRefreshTokenId);
        return Ok(new ResponseDto(accessToken, newRefreshToken));
    }
}