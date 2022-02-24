namespace Meetups.Backend.Application.Features.Feed.GetMeetups;

using System.Linq;
using System.Threading.Tasks;
using Meetup.Contract.Models.Features.Feed.GetMeetups;
using Meetup.Contract.Routing;
using Meetups.Backend.Application.Features.Seedwork;
using Meetups.Backend.Application.Modules.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Tags(Tags.Feed)]
public class Controller : ApiControllerBase
{
    private readonly IApplicationContext context;

    public Controller(IApplicationContext context) =>
        this.context = context;

    /// <summary>Get several meetups.</summary>
    /// <response code="200">Retrieved meetups successfully.</response>
    /// <response code="400">Validation failed for pagination options.</response>
    [HttpGet(Routes.Feed.GetMeetups)]
    [ProducesResponseType(typeof(ResponseDto[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllMeetups([FromQuery] RequestDto request)
    {
        var meetups = await context.Meetups
            .AsNoTracking()
            .Include(meetup => meetup.SignedUpGuests)
            .Include(meetup => meetup.Place.City)
            .ApplyFilters(request.Filters)
            .OrderBy(request.Pagination.OrderBy)
            .Paginate(request.Pagination)
            .ToListAsync();
        
        var response = meetups.Select(Mappings.ToResponseDto);
        return Ok(response);
    }
}