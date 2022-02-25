namespace Meetups.Application.Features.Auth.ChangeCredentials;

using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;
using Meetups.Application.Features.Shared.Infrastructure;
using Meetups.Application.Modules.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Tags("Auth")]
public class Controller : ApiControllerBase
{
    private readonly IApplicationContext context;

    public Controller(IApplicationContext context) =>
        this.context = context;

    /// <summary>Change current user signing credentials.</summary>
    /// <param name="request">New credentials.</param>
    /// <response code="200">Credentials changed successfully.</response>
    /// <response code="409">Username already taken.</response>
    [Authorize]
    [HttpPut("auth/credentials")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ChangeCredentials([FromBody] RequestDto request)
    {
        var usernameTaken = await context.Users
            .Include(user => user.RefreshTokens)
            .Where(user => user.Id != CurrentUser.UserId)
            .AnyAsync(user => user.Username == request.Username);
        if (usernameTaken)
        {
            return Conflict();
        }
        
        var user = await context.Users.SingleAsync(user => user.Id == CurrentUser.UserId);
        user.ChangeCredentials(request.Username, BCrypt.HashPassword(request.Password));
        user.RevokeAllRefreshTokens();
        await context.SaveChangesAsync();
        
        return Ok();
    }
}