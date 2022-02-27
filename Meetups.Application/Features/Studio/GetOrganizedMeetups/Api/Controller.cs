namespace Meetups.Application.Features.Studio.GetOrganizedMeetups.Api;

using System.Linq;
using System.Threading.Tasks;
using Meetups.Application.Features.Shared.Contracts.Auth;
using Meetups.Application.Features.Shared.Contracts.PrimitiveDtos;
using Meetups.Application.Features.Shared.Infrastructure.Api;
using Meetups.Application.Features.Studio.GetOrganizedMeetups.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[Tags("Studio")]
public class Controller : ApiControllerBase
{
    private readonly RequestHandler requestHandler;

    public Controller(RequestHandler requestHandler) =>
        this.requestHandler = requestHandler;

    /// <summary>Get a list of all meetups organized by the current user.</summary>
    /// <response code="200">Meetups organized by the current user.</response>
    [Authorize(Roles = UserRoles.Organizer)]
    [HttpGet("studio/organized")]
    [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
    public Task<IActionResult> GetOrganizedMeetups() =>
        ApiPipeline
            .CreateRequest(new Request(CurrentUser.UserId))
            .HandleRequestAsync(requestHandler)
            .ToApiResponse(
                onSuccess: result =>
                {
                    var meetups = result.Select(meetup => new MeetupDto(
                        id: meetup.Id,
                        topic: meetup.Topic,
                        place: new MeetupPlaceDto(
                            cityId: meetup.Place.CityId,
                            cityName: meetup.Place.CityName,
                            address: meetup.Place.Address),
                        startTime: meetup.StartTime,
                        duration: new MeetupDurationDto(
                            hours: meetup.Duration.Hours,
                            minutes: meetup.Duration.Minutes),
                        signedUpGuestsCount: meetup.SignedUpGuestsCount));
                    return Ok(new ResponseDto(meetups));
                },
                onFailure: _ => InternalServerError());
}