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
    /// <param name="request">User credentials.</param>
    /// <response code="200">User authenticated successfully.</response>
    /// <response code="404">User with specified username does not exist.</response>
    /// <response code="409">Incorrect password provided.</response>
    [HttpPost("auth/authenticate")]
    [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public Task<IActionResult> AuthenticateUser([FromBody] RequestDto request) =>
        ApiPipeline
            .CreateRequest(new Request(
                Username: request.Username,
                Password: request.Password))
            .HandleRequestAsync(requestHandler)
            .ToApiResponse(
                onSuccess: result =>
                {
                    var payload = new ResponseDto(result.AccessToken, result.RefreshToken);
                    return Ok(payload);
                },
                onFailure: errorType => errorType switch
                {
                    ErrorTypes.UserDoesNotExist => NotFound(),
                    ErrorTypes.IncorrectPassword => Conflict(),
                    _ => InternalServerError()
                });
}