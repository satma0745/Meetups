namespace Meetups.Application.Features.Studio.RegisterNewCity.Api;

using System.Threading.Tasks;
using Meetups.Application.Features.Shared.Contracts.Auth;
using Meetups.Application.Features.Shared.Infrastructure.Api;
using Meetups.Application.Features.Studio.RegisterNewCity.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[Tags("Studio")]
public class Controller : ApiControllerBase
{
    private readonly RequestHandler requestHandler;

    public Controller(RequestHandler requestHandler) =>
        this.requestHandler = requestHandler;

    /// <summary>Register a new city.</summary>
    /// <param name="requestDto">DTO to create city based on.</param>
    /// <response code="200">New city was registered successfully.</response>
    /// <response code="409">City with te same name already exists.</response>
    [Authorize(Roles = UserRoles.Organizer)]
    [HttpPost("studio/new-city")]
    [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterNewCity([FromBody] RequestDto requestDto)
    {
        var internalRequest = requestDto.ToInternalRequest();
        var internalResponse = await requestHandler.HandleRequest(internalRequest);
        return (internalResponse.Success, internalResponse.Payload, internalResponse.ErrorType) switch
        {
            (true, var internalResult, _) => Ok(internalResult.ToApiResponse()),
            (false, _, ErrorTypes.NameAlreadyTaken) => Conflict(),
            _ => InternalServerError()
        };
    }
}