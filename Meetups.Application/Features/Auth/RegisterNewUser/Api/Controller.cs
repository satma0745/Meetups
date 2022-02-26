namespace Meetups.Application.Features.Auth.RegisterNewUser.Api;

using System.Threading.Tasks;
using Meetups.Application.Features.Auth.RegisterNewUser.Internal;
using Meetups.Application.Features.Shared.Infrastructure.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[Tags("Auth")]
public class Controller : ApiControllerBase
{
    private readonly RequestHandler requestHandler;

    public Controller(RequestHandler requestHandler) =>
        this.requestHandler = requestHandler;
    
    /// <summary>Register a new user.</summary>
    /// <param name="requestDto">DTO to create user from.</param>
    /// <response code="200">New user was created successfully.</response>
    /// <response code="400">Validation failed for DTO.</response>
    /// <response code="409">User with the same username already exists.</response>
    [HttpPost("auth/register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterNewUser([FromBody] RequestDto requestDto)
    {
        var internalRequest = requestDto.ToInternalRequest();
        var internalResponse = await requestHandler.HandleRequest(internalRequest);
        return (internalResponse.Success, internalResponse.ErrorType) switch
        {
            (true, _) => Ok(),
            (false, ErrorTypes.UsernameAlreadyTaken) => Conflict(),
            _ => InternalServerError()
        };
    }
}