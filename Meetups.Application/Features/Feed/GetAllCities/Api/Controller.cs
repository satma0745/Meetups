namespace Meetups.Application.Features.Feed.GetAllCities.Api;

using System.Linq;
using System.Threading.Tasks;
using Meetups.Application.Features.Feed.GetAllCities.Internal;
using Meetups.Application.Features.Shared.Infrastructure.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[Tags("Feed")]
public class Controller : ApiControllerBase
{
    private readonly RequestHandler requestHandler;

    public Controller(RequestHandler requestHandler) =>
        this.requestHandler = requestHandler;

    /// <summary>Get all existing cities.</summary>
    /// <response code="200">Cities retrieved successfully.</response>
    [HttpGet("feed/all-cities")]
    [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
    public Task<IActionResult> GetAllCities() =>
        ApiPipeline
            .CreateRequest(new Request())
            .HandleRequestAsync(requestHandler)
            .ToApiResponse(
                onSuccess: result =>
                {
                    var cities = result.Select(city => new CityDto(
                        id: city.Id,
                        name: city.Name));
                    return Ok(new ResponseDto(cities));
                },
                onFailure: _ => InternalServerError());
}