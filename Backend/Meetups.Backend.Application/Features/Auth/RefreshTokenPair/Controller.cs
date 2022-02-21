﻿namespace Meetups.Backend.Application.Features.Auth.RefreshTokenPair;

using System;
using System.Threading.Tasks;
using AutoMapper;
using Meetup.Contract.Models.Primitives;
using Meetup.Contract.Models.Tokens;
using Meetup.Contract.Routing;
using Meetups.Backend.Application.Seedwork;
using Meetups.Backend.Entities.User;
using Meetups.Backend.Persistence.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Tags(Tags.Auth)]
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
    [HttpPost(Routes.Auth.RefreshTokenPair)]
    [ProducesResponseType(typeof(TokenPairDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RefreshTokenPair([FromBody] string oldRefreshToken)
    {
        if (!tokenHelper.TryParseToken(oldRefreshToken, out var claims))
        {
            return BadRequest();
        }
        var currentUserId = Guid.Parse(claims[RefreshTokenPayload.UserIdClaim]);
        var refreshTokenId = Guid.Parse(claims[RefreshTokenPayload.TokenIdClaim]);

        var currentUser = await Context.Users
            .Include(user => user.RefreshTokens)
            .SingleOrDefaultAsync(user => user.Id == currentUserId);
        var oldPersistedRefreshToken = await Context.RefreshTokens
            .SingleOrDefaultAsync(token => token.TokenId == refreshTokenId);
        if (currentUser is null || oldPersistedRefreshToken is null)
        {
            // Cannot issue token for user that does not even exist (was deleted)
            // Cannot use refresh token that is not persisted to a db (fake or used)
            return BadRequest();
        }

        var newPersistedRefreshToken = new RefreshToken(tokenId: Guid.NewGuid(), bearerId: currentUserId);
        currentUser.ReplaceRefreshToken(oldPersistedRefreshToken, newPersistedRefreshToken);
        await Context.SaveChangesAsync();

        var (accessToken, newRefreshToken) = tokenHelper.IssueTokenPair(currentUser, newPersistedRefreshToken.TokenId);        
        var tokenPairDto = new TokenPairDto
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken
        };
        return Ok(tokenPairDto);
    }
}