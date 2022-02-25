namespace Meetups.Application.Features.Auth.GetCurrentUserInfo;

using System.Threading.Tasks;
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
    
    /// <summary>Retrieve info about the current user.</summary>
    /// <response code="200">Information was retrieved successfully.</response>
    /// <response code="401">Unauthorized request.</response>
    [Authorize]
    [HttpGet("auth/who-am-i")]
    [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetCurrentUserInfo()
    {
        var user = await context.Users.SingleOrDefaultAsync(user => user.Id == CurrentUser.UserId);
        if (user is null)
        {
            return Unauthorized();
        }

        var response = user.ToResponseDto();
        return Ok(response);
    }
}