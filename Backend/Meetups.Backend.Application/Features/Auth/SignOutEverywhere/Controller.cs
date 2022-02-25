namespace Meetups.Backend.Application.Features.Auth.SignOutEverywhere;

using System.Threading.Tasks;
using Meetups.Backend.Application.Features.Seedwork;
using Meetups.Backend.Application.Modules.Persistence;
using Meetups.Contract.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Tags(Tags.Auth)]
public class Controller : ApiControllerBase
{
    private readonly IApplicationContext context;

    public Controller(IApplicationContext context) =>
        this.context = context;

    /// <summary>Sign out user everywhere.</summary>
    /// <response code="200">User signed out successfully.</response>
    [Authorize]
    [HttpPost(Routes.Auth.SignOutEverywhere)]
    public async Task<IActionResult> SignOutEverywhere()
    {
        var currentUser = await context.Users
            .Include(user => user.RefreshTokens)
            .SingleAsync(user => user.Id == CurrentUser.UserId);
        
        currentUser.RevokeAllRefreshTokens();
        await context.SaveChangesAsync();

        return Ok();
    }
}