namespace Meetups.Application.Features.Feed.GetSpecificMeetup.Api;

using System;
using System.Threading.Tasks;
using Meetups.Application.Features.Feed.GetSpecificMeetup.Internal;
using Meetups.Application.Features.Shared.Contracts.PrimitiveDtos;
using Meetups.Application.Features.Shared.Infrastructure.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[Tags("Feed")]
public class Controller : ApiControllerBase
{
    private readonly RequestHandler requestHandler;

    public Controller(RequestHandler requestHandler) =>
        this.requestHandler = requestHandler;

    /// <summary>Get specific meetup (with the specified id).</summary>
    /// <param name="meetupId">Id of the meetup of interest.</param>
    /// <response code="200">Meetup was retrieved successfully.</response>
    /// <response code="404">Meetup with the specified id does not exist.</response>
    [HttpGet("feed/{meetupId:guid}")]
    [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<IActionResult> GetSpecificMeetup([FromRoute] Guid meetupId) =>
        ApiPipeline
            .CreateRequest(new Request(meetupId))
            .HandleRequestAsync(requestHandler)
            .ToApiResponse(
                onSuccess: result =>
                {
                    var payload = new ResponseDto(
                        id: result.Id,
                        topic: result.Topic,
                        place: new MeetupPlaceDto(
                            cityId: result.Place.CityId,
                            cityName: result.Place.CityName,
                            address: result.Place.Address),
                        startTime: result.StartTime,
                        duration: new MeetupDurationDto(
                            hours: result.Duration.Hours,
                            minutes: result.Duration.Minutes),
                        signedUpGuestsCount: result.SignedUpGuestsCount);
                    return Ok(payload);
                },
                onFailure: errorType => errorType switch
                {
                    ErrorTypes.MeetupDoesNotExist => NotFound(),
                    _ => InternalServerError()
                });
}