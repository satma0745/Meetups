namespace Meetups.Application.Features.Feed.GetMeetups.Api;

using System.Linq;
using System.Threading.Tasks;
using Meetups.Application.Features.Feed.GetMeetups.Internal;
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

    /// <summary>Get several meetups.</summary>
    /// <response code="200">Retrieved meetups successfully.</response>
    /// <response code="400">Validation failed for pagination options.</response>
    [HttpGet("feed")]
    [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
    public Task<IActionResult> GetAllMeetups([FromQuery] RequestDto request) =>
        ApiPipeline
            .CreateRequest(new Request(
                Pagination: new PaginationModel(
                    PageNumber: request.Pagination.PageNumber,
                    PageSize: request.Pagination.PageSize,
                    OrderBy: Mappings.ToOrderingOption(request.Pagination.OrderBy)),
                Filters: new FiltersModel(
                    CityId: request.Filters.CityId,
                    Search: request.Filters.Search)))
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