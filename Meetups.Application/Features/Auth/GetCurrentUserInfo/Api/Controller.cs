namespace Meetups.Application.Features.Auth.GetCurrentUserInfo.Api;

using System.Threading.Tasks;
using Meetups.Application.Features.Auth.GetCurrentUserInfo.Internal;
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
    
    /// <summary>Retrieve info about the current user.</summary>
    /// <response code="200">Information was retrieved successfully.</response>
    /// <response code="401">Unauthorized request.</response>
    [Authorize]
    [HttpGet("auth/who-am-i")]
    [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetCurrentUserInfo()
    {
        var internalRequest = new Request(CurrentUser.UserId);
        var internalResponse = await requestHandler.HandleRequest(internalRequest);
        return (internalResponse.Success, internalResponse.Payload, internalResponse.ErrorType) switch
        {
            (true, var internalResult, _) => Ok(internalResult.ToApiResponse()),
            (false, _, ErrorTypes.UserDoesNotExist) => Unauthorized(),
            _ => InternalServerError()
        };
    }
}