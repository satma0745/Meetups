namespace Meetups.Application.Features.Studio.UpdateMeetupDescription.Api;

using System;
using System.Threading.Tasks;
using Meetups.Application.Features.Shared.Auth;
using Meetups.Application.Features.Studio.UpdateMeetupDescription.Internal;
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

    /// <summary>Updates description of a specific meetup (with the specified ID).</summary>
    /// <param name="meetupId">Meetup ID to update description of.</param>
    /// <param name="request">DTO with the updated meetup description.</param>
    /// <response code="200">Meetup description was updated successfully.</response>
    /// <response code="403">Only the organizer of a meetup can update it's description.</response>
    /// <response code="404">Specified meetup does not exist.</response>
    [Authorize(Roles = UserRoles.Organizer)]
    [HttpPut("studio/{meetupId:guid}/description")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<IActionResult> RescheduleMeetup([FromRoute] Guid meetupId, [FromBody] RequestDto request) =>
        ApiPipeline
            .CreateRequest(new Request(
                MeetupId: meetupId,
                Topic: request.Topic,
                CurrentUserId: CurrentUser.UserId))
            .HandleRequestAsync(requestHandler)
            .ToApiResponse(
                onSuccess: _ => Ok(),
                onFailure: errorType => errorType switch
                {
                    ErrorTypes.MeetupDoesNotExist => NotFound(),
                    ErrorTypes.AccessViolation => Forbid(),
                    _ => InternalServerError()
                });
}