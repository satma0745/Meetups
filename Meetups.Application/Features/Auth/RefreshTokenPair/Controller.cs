namespace Meetups.Application.Features.Auth.RefreshTokenPair;

using System;
using System.Threading.Tasks;
using Meetups.Application.Features.Shared.Contracts;
using Meetups.Application.Features.Shared.Contracts.PrimitiveDtos;
using Meetups.Application.Features.Shared.Infrastructure;
using Meetups.Application.Modules.Auth;
using Meetups.Application.Modules.Persistence;
using Meetups.Domain.Entities.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Tags("Auth")]
public class Controller : ApiControllerBase
{
    private readonly IApplicationContext context;
    private readonly ITokenHelper tokenHelper;

    public Controller(IApplicationContext context, ITokenHelper tokenHelper)
    {
        this.context = context;
        this.tokenHelper = tokenHelper;
    }

    /// <summary>Re-issue token pair using refresh token.</summary>
    /// <param name="oldRefreshToken">Refresh token.</param>
    /// <response code="200">Token pair was successfully re-issued.</response>
    /// <response code="400">Fake, damaged, expired or used refresh token was provided.</response>
    [HttpPost("auth/refresh")]
    [ProducesResponseType(typeof(TokenPairDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RefreshTokenPair([FromBody] string oldRefreshToken)
    {
        if (!tokenHelper.TryParseRefreshToken(oldRefreshToken, out var oldRefreshTokenPayload))
        {
            return BadRequest();
        }

        var currentUser = await context.Users
            .Include(user => user.RefreshTokens)
            .SingleOrDefaultAsync(user => user.Id == oldRefreshTokenPayload.BearerId);
        if (currentUser is null)
        {
            // Cannot issue token for user that does not even exist (was deleted)
            return BadRequest();
        }
        if (!currentUser.TryGetRefreshToken(oldRefreshTokenPayload.TokenId, out var oldPersistedRefreshToken))
        {
            // Cannot use refresh token that is not persisted to a db (fake or used)
            return BadRequest();
        }

        var newPersistedRefreshToken = new RefreshToken(tokenId: Guid.NewGuid(), oldRefreshTokenPayload.BearerId);
        currentUser.ReplaceRefreshToken(oldPersistedRefreshToken, newPersistedRefreshToken);
        await context.SaveChangesAsync();

        var response = tokenHelper
            .IssueTokenPair(currentUser, newPersistedRefreshToken.TokenId)
            .ToTokenPairDto();
        return Ok(response);
    }
}