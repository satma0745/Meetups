namespace Meetups.Application.Features.Feed.CancelMeetupSignup.Api;

using System;
using System.Threading.Tasks;
using Meetups.Application.Features.Feed.CancelMeetupSignup.Internal;
using Meetups.Application.Features.Shared.Auth;
using Meetups.Application.Seedwork.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[Tags("Feed")]
public class Controller : ApiControllerBase
{
    private readonly RequestHandler requestHandler;

    public Controller(RequestHandler requestHandler) =>
        this.requestHandler = requestHandler;

    /// <summary>Cancel meetup signup.</summary>
    /// <param name="meetupId">Id of the meetup to cancel signup for.</param>
    /// <response code="200">Signup for a meetup canceled successfully.</response>
    /// <response code="404">Meetup with the specified id does not exist.</response>
    /// <response code="409">User has to sign up for the specified meetup in the first place.</response>
    [Authorize(Roles = UserRoles.Guest)]
    [HttpPost("feed/{meetupId:guid}/cancel-sign-up")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public Task<IActionResult> CancelMeetupSignup([FromRoute] Guid meetupId) =>
        ApiPipeline
            .CreateRequest(new Request(
                MeetupId: meetupId,
                CurrentUserId: CurrentUser.UserId))
            .HandleRequestAsync(requestHandler)
            .ToApiResponse(
                onSuccess: _ => Ok(),
                onFailure: errorType => errorType switch
                {
                    ErrorTypes.MeetupDoesNotExist => NotFound(),
                    ErrorTypes.NotSignedUp => Conflict(),
                    _ => InternalServerError()
                });
}