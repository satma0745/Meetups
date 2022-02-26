namespace Meetups.Application.Features.Auth.AuthenticateUser.Api;

using System.Threading.Tasks;
using Meetups.Application.Features.Auth.AuthenticateUser.Internal;
using Meetups.Application.Features.Shared.Infrastructure.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[Tags("Auth")]
public class Controller : ApiControllerBase
{
    private readonly RequestHandler requestHandler;

    public Controller(RequestHandler requestHandler) =>
        this.requestHandler = requestHandler;

    /// <summary>Issue token pair for provided user credentials.</summary>
    /// <param name="requestDto">User credentials.</param>
    /// <response code="200">User authenticated successfully.</response>
    /// <response code="404">User with specified username does not exist.</response>
    /// <response code="409">Incorrect password provided.</response>
    [HttpPost("auth/authenticate")]
    [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AuthenticateUser([FromBody] RequestDto requestDto)
    {
        var internalRequest = requestDto.ToInternalRequest();
        var internalResponse = await requestHandler.HandleRequest(internalRequest);
        return (internalResponse.Success, internalResponse.Payload, internalResponse.ErrorType) switch
        {
            (true, var internalResult, _) => Ok(internalResult.ToApiResponse()),
            (false, _, ErrorTypes.UserDoesNotExist) => NotFound(),
            (false, _, ErrorTypes.IncorrectPassword) => Conflict(),
            _ => InternalServerError()
        };
    }
}