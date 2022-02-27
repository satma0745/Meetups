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
    /// <param name="request">DTO to create meetup from.</param>
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
    public Task<IActionResult> RegisterNewMeetup([FromBody] RequestDto request) =>
        ApiPipeline
            .CreateRequest(new Request(
                Topic: request.Topic,
                Place: new MeetupPlaceModel(
                    CityId: request.Place.CityId,
                    Address: request.Place.Address),
                StartTime: request.StartTime,
                Duration: new MeetupDurationModel(
                    Hours: request.Duration.Hours,
                    Minutes: request.Duration.Minutes),
                OrganizerId: CurrentUser.UserId))
            .HandleRequestAsync(requestHandler)
            .ToApiResponse(
                onSuccess: _ => Ok(),
                onFailure: errorType => errorType switch
                {
                    ErrorTypes.TopicAlreadyTaken => Conflict(),
                    ErrorTypes.CityDoesNotExist => NotFound(),
                    _ => InternalServerError()
                });
}