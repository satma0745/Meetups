namespace Meetups.Application.Features.Studio.RegisterNewMeetup.Api;

using System.Threading.Tasks;
using Meetups.Application.Features.Shared.Contracts.Auth;
using Meetups.Application.Features.Shared.Infrastructure.Api;
using Meetups.Application.Features.Studio.RegisterNewMeetup.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[Tags("Studio")]
public class Controller : ApiControllerBase
{
    private readonly RequestHandler requestHandler;

    public Controller(RequestHandler requestHandler) =>
        this.requestHandler = requestHandler;

    /// <summary>Register new meetup.</summary>
    /// <param name="requestDto">DTO to create meetup from.</param>
    /// <response code="200">New meetup was created successfully.</response>
    /// <response code="400">Validation failed for DTO.</response>
    /// <response code="404">Specified city does not exist.</response>
    /// <response code="409">The exact same topic for the meetup has already been taken up.</response>
    [Authorize(Roles = UserRoles.Organizer)]
    [HttpPost("studio/new")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterNewMeetup([FromBody] RequestDto requestDto)
    {
        var internalRequest = requestDto.ToInternalRequest(CurrentUser);
        var internalResponse = await requestHandler.HandleRequest(internalRequest);
        return (internalResponse.Success, internalResponse.ErrorType) switch
        {
            (true, _) => Ok(),
            (false, ErrorTypes.TopicAlreadyTaken) => Conflict(),
            (false, ErrorTypes.CityDoesNotExist) => NotFound(),
            _ => InternalServerError()
        };
    }
}