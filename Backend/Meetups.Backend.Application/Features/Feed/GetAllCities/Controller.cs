namespace Meetups.Backend.Application.Features.Feed.GetAllCities;

using System.Linq;
using System.Threading.Tasks;
using Meetup.Contract.Models.Features.Feed.GetAllCities;
using Meetup.Contract.Routing;
using Meetups.Backend.Application.Seedwork;
using Meetups.Backend.Persistence.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Tags(Tags.Feed)]
public class Controller : ApiControllerBase
{
    private readonly ApplicationContext context;

    public Controller(ApplicationContext context) =>
        this.context = context;

    /// <summary>Get all existing cities.</summary>
    /// <response code="200">Cities retrieved successfully.</response>
    [HttpGet(Routes.Feed.GetAllCities)]
    [ProducesResponseType(typeof(ResponseDto[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllCities()
    {
        var cities = await context.Cities
            .AsNoTracking()
            .ToListAsync();

        var response = cities.Select(Mappings.ToResponseDto);
        return Ok(response);
    }
}