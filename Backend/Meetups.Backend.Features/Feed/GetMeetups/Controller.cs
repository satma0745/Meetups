namespace Meetups.Backend.Features.Feed.GetMeetups;

using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Meetups.Backend.Features.Shared;
using Meetups.Backend.Persistence.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Tags("Feed")]
public class Controller : ApiControllerBase
{
    public Controller(ApplicationContext context, IMapper mapper)
        : base(context, mapper)
    {
    }

    /// <summary>Get several meetups.</summary>
    /// <response code="200">Retrieved meetups successfully.</response>
    /// <response code="400">Validation failed for pagination options.</response>
    [HttpGet("feed")]
    [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllMeetups([FromQuery] RequestDto request)
    {
        var meetups = await Context.Meetups
            .AsNoTracking()
            .Include(meetup => meetup.SignedUpGuests)
            .ApplySearch($"%{request.Search}%")
            .OrderBy(request.OrderBy)
            .Paginate(request.PageNumber, request.PageSize)
            .ToListAsync();
        
        var response = Mapper.Map<IEnumerable<ResponseDto>>(meetups);
        return Ok(response);
    }
}