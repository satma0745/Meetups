namespace Meetups.Backend.Application.Features.Auth.AuthenticateUser;

using System;
using System.Threading.Tasks;
using BCrypt.Net;
using Meetup.Contract.Models.Features.Auth.AuthenticateUser;
using Meetup.Contract.Models.Primitives;
using Meetup.Contract.Routing;
using Meetups.Backend.Application.Features.Seedwork;
using Meetups.Backend.Application.Modules.Auth;
using Meetups.Backend.Application.Modules.Persistence;
using Meetups.Backend.Domain.Entities.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Tags(Tags.Auth)]
public class Controller : ApiControllerBase
{
    private readonly IApplicationContext context;
    private readonly ITokenHelper tokenHelper;

    public Controller(IApplicationContext context, ITokenHelper tokenHelper)
    {
        this.context = context;
        this.tokenHelper = tokenHelper;
    }

    /// <summary>Issue token pair for provided user credentials.</summary>
    /// <param name="request">User credentials.</param>
    /// <response code="200">User authenticated successfully.</response>
    /// <response code="404">User with specified username does not exist.</response>
    /// <response code="409">Incorrect password provided.</response>
    [HttpPost(Routes.Auth.AuthenticateUser)]
    [ProducesResponseType(typeof(TokenPairDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AuthenticateUser([FromBody] RequestDto request)
    {
        var user = await context.Users
            .Include(user => user.RefreshTokens)
            .SingleOrDefaultAsync(user => user.Username == request.Username);
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
        var persistedRefreshToken = new RefreshToken(tokenId: Guid.NewGuid(), bearerId: user.Id);
        user.AddRefreshToken(persistedRefreshToken);
        await context.SaveChangesAsync();

        var response = tokenHelper
            .IssueTokenPair(user, persistedRefreshToken.TokenId)
            .ToTokenPairDto();
        return Ok(response);
    }
}