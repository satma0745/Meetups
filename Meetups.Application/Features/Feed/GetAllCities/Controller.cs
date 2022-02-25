namespace Meetups.Application.Features.Feed.GetAllCities;

using System.Linq;
using System.Threading.Tasks;
using Meetups.Application.Features.Shared.Infrastructure;
using Meetups.Application.Modules.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Tags("Feed")]
public class Controller : ApiControllerBase
{
    private readonly IApplicationContext context;

    public Controller(IApplicationContext context) =>
        this.context = context;

    /// <summary>Get all existing cities.</summary>
    /// <response code="200">Cities retrieved successfully.</response>
    [HttpGet("feed/all-cities")]
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