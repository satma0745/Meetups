namespace Meetups.Application.Features.Auth.ChangeCredentials.Api;

using System.Threading.Tasks;
using Meetups.Application.Features.Auth.ChangeCredentials.Internal;
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

    /// <summary>Change current user signing credentials.</summary>
    /// <param name="requestDto">New credentials.</param>
    /// <response code="200">Credentials changed successfully.</response>
    /// <response code="409">Username already taken.</response>
    [Authorize]
    [HttpPut("auth/credentials")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ChangeCredentials([FromBody] RequestDto requestDto)
    {
        var internalRequest = requestDto.ToInternalRequest(CurrentUser);
        var internalResponse = await requestHandler.HandleRequest(internalRequest);
        return (internalResponse.Success, internalResponse.ErrorType) switch
        {
            (true, _) => Ok(),
            (false, ErrorTypes.UsernameAlreadyTaken) => Conflict(),
            _ => InternalServerError()
        };
    }
}