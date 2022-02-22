namespace Meetups.Backend.Application.Features.Auth.GetCurrentUserInfo;

using System.Threading.Tasks;
using Meetup.Contract.Models.Features.Auth.GetCurrentUserInfo;
using Meetup.Contract.Routing;
using Meetups.Backend.Application.Seedwork;
using Meetups.Backend.Persistence.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Tags(Tags.Auth)]
public class Controller : ApiControllerBase
{
    private readonly ApplicationContext context;

    public Controller(ApplicationContext context) =>
        this.context = context;
    
    /// <summary>Retrieve info about the current user.</summary>
    /// <response code="200">Information was retrieved successfully.</response>
    /// <response code="401">Unauthorized request.</response>
    [Authorize]
    [HttpGet(Routes.Auth.GetCurrentUserInfo)]
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