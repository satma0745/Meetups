namespace Meetups.Application.Features.Studio.RescheduleMeetup.Api;

using System;
using System.Threading.Tasks;
using Meetups.Application.Features.Shared.Contracts.Auth;
using Meetups.Application.Features.Shared.Infrastructure.Api;
using Meetups.Application.Features.Studio.RescheduleMeetup.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[Tags("Studio")]
public class Controller : ApiControllerBase
{
    private readonly RequestHandler requestHandler;

    public Controller(RequestHandler requestHandler) =>
        this.requestHandler = requestHandler;

    /// <summary>Reschedules specific meetup (with the specified ID).</summary>
    /// <param name="meetupId">Meetup ID to reschedule.</param>
    /// <param name="requestDto">DTO with the updated scheduling information.</param>
    /// <response code="200">Meetup was rescheduled successfully.</response>
    /// <response code="403">Only the organizer of a meetup can reschedule a specific meetup.</response>
    /// <response code="404">Specified meetup or city does not exist.</response>
    [Authorize(Roles = UserRoles.Organizer)]
    [HttpPost("studio/{meetupId:guid}/reschedule")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RescheduleMeetup([FromRoute] Guid meetupId, [FromBody] RequestDto requestDto)
    {
        var internalRequest = requestDto.ToInternalRequest(meetupId, CurrentUser);
        var internalResponse = await requestHandler.HandleRequest(internalRequest);
        return (internalResponse.Success, internalResponse.ErrorType) switch
        {
            (true, _) => Ok(),
            (false, ErrorTypes.MeetupDoesNotExist) => NotFound(),
            (false, ErrorTypes.AccessViolation) => Forbid(),
            (false, ErrorTypes.CityDoesNotExist) => NotFound(),
            _ => InternalServerError()
        };
    }
}