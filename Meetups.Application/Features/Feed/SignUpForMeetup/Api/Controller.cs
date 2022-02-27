namespace Meetups.Application.Features.Feed.SignUpForMeetup.Api;

using System;
using System.Threading.Tasks;
using Meetups.Application.Features.Feed.SignUpForMeetup.Internal;
using Meetups.Application.Features.Shared.Contracts.Auth;
using Meetups.Application.Features.Shared.Infrastructure.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[Tags("Feed")]
public class Controller : ApiControllerBase
{
    private readonly RequestHandler requestHandler;

    public Controller(RequestHandler requestHandler) =>
        this.requestHandler = requestHandler;

    /// <summary>Sign up for an incoming meetup.</summary>
    /// <param name="meetupId">Meetup ID to sign up for.</param>
    /// <response code="200">Successfully signed up for a meetup.</response>
    /// <response code="404">Meetup with the specified ID does not exist.</response>
    /// <response code="409">You have already signed up for that meetup.</response>
    [Authorize(Roles = UserRoles.Guest)]
    [HttpPost("feed/{meetupId:guid}/sign-up")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public Task<IActionResult> SignUpForMeetup([FromRoute] Guid meetupId) =>
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
                    ErrorTypes.AlreadySignedUp => Conflict(),
                    _ => InternalServerError()
                });
}