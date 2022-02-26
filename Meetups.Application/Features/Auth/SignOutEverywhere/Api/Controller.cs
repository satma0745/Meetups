namespace Meetups.Application.Features.Auth.SignOutEverywhere.Api;

using System.Threading.Tasks;
using Meetups.Application.Features.Auth.SignOutEverywhere.Internal;
using Meetups.Application.Features.Shared.Infrastructure.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[Tags("Auth")]
public class Controller : ApiControllerBase
{
    private readonly RequestHandler requestHandler;

    public Controller(RequestHandler requestHandler) =>
        this.requestHandler = requestHandler;

    /// <summary>Sign out user everywhere.</summary>
    /// <response code="200">User signed out successfully.</response>
    [Authorize]
    [HttpPost("auth/sign-out-everywhere")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SignOutEverywhere()
    {
        var internalRequest = new Request(CurrentUser.UserId);
        var internalResponse = await requestHandler.HandleRequest(internalRequest);
        return internalResponse.Success switch
        {
            true => Ok(),
            false => InternalServerError()
        };
    }
}