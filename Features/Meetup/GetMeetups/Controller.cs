namespace Meetups.Features.Meetup.GetMeetups;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Meetups.Features.Shared;
using Meetups.Persistence.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Tags("Meetup")]
public class Controller : ApiControllerBase
{
    public Controller(ApplicationContext context, IMapper mapper)
        : base(context, mapper)
    {
    }

    /// <summary>Get several meetups.</summary>
    /// <response code="200">Retrieved meetups successfully.</response>
    /// <response code="400">Validation failed for pagination options.</response>
    [HttpGet("meetups")]
    [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllMeetups([FromQuery] RequestDto request)
    {
        var filteredMeetups = Context.Meetups
            .AsNoTracking()
            .Where(meetup => EF.Functions.Like(meetup.Topic, $"%{request.Search}%") ||
                             EF.Functions.Like(meetup.Place, $"%{request.Search}%"));
        var orderedMeetups = request.OrderBy switch
        {
            "topic_asc" => filteredMeetups.OrderBy(meetup => meetup.Topic),
            "topic_desc" => filteredMeetups.OrderByDescending(meetup => meetup.Topic),
            "stime_asc" => filteredMeetups.OrderBy(meetup => meetup.StartTime),
            "stime_desc" => filteredMeetups.OrderByDescending(meetup => meetup.StartTime),
            _ => filteredMeetups
        };
        var meetups = await orderedMeetups
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();
        
        var response = Mapper.Map<IEnumerable<ResponseDto>>(meetups);
        return Ok(response);
    }
}