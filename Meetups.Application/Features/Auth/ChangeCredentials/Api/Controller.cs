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
    /// <param name="request">New credentials.</param>
    /// <response code="200">Credentials changed successfully.</response>
    /// <response code="409">Username already taken.</response>
    [Authorize]
    [HttpPut("auth/credentials")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public Task<IActionResult> ChangeCredentials([FromBody] RequestDto request) =>
        ApiPipeline
            .CreateRequest(new Request(
                UserId: CurrentUser.UserId,
                Username: request.Username,
                Password: request.Password))
            .HandleRequestAsync(requestHandler)
            .ToApiResponse(
                onSuccess: _ => Ok(),
                onFailure: errorType => errorType switch
                {
                    ErrorTypes.UsernameAlreadyTaken => Conflict(),
                    _ => InternalServerError()
                });
}