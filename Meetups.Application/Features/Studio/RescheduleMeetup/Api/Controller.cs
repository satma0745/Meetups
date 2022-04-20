namespace Meetups.Application.Features.Studio.RescheduleMeetup.Api;

using System;
using System.Threading.Tasks;
using Meetups.Application.Features.Shared.Auth;
using Meetups.Application.Features.Studio.RescheduleMeetup.Internal;
using Meetups.Application.Seedwork.Api;
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
    /// <param name="request">DTO with the updated scheduling information.</param>
    /// <response code="200">Meetup was rescheduled successfully.</response>
    /// <response code="403">Only the organizer of a meetup can reschedule a specific meetup.</response>
    /// <response code="404">Specified meetup or city does not exist.</response>
    [Authorize(Roles = UserRoles.Organizer)]
    [HttpPost("studio/{meetupId:guid}/reschedule")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<IActionResult> RescheduleMeetup([FromRoute] Guid meetupId, [FromBody] RequestDto request) =>
        ApiPipeline
            .CreateRequest(new Request(
                MeetupId: meetupId,
                Place: new MeetupPlaceModel(
                    CityId: request.Place.CityId,
                    Address: request.Place.Address),
                StartTime: request.StartTime,
                Duration: new MeetupDurationModel(
                    Hours: request.Duration.Hours,
                    Minutes: request.Duration.Minutes),
                CurrentUserId: CurrentUser.UserId))
            .HandleRequestAsync(requestHandler)
            .ToApiResponse(
                onSuccess: _ => Ok(),
                onFailure: errorType => errorType switch
                {
                    ErrorTypes.MeetupDoesNotExist => NotFound(),
                    ErrorTypes.AccessViolation => Forbid(),
                    ErrorTypes.CityDoesNotExist => NotFound(),
                    _ => InternalServerError()
                });
}