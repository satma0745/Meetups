namespace Meetups.Backend.Application.Features.Feed.GetMeetups;

using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Meetup.Contract.Models.Features.Feed.GetMeetups;
using Meetup.Contract.Routing;
using Meetups.Backend.Application.Seedwork;
using Meetups.Backend.Persistence.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Tags(Tags.Feed)]
public class Controller : ApiControllerBase
{
    public Controller(ApplicationContext context, IMapper mapper)
        : base(context, mapper)
    {
    }

    /// <summary>Get several meetups.</summary>
    /// <response code="200">Retrieved meetups successfully.</response>
    /// <response code="400">Validation failed for pagination options.</response>
    [HttpGet(Routes.Feed.GetMeetups)]
    [ProducesResponseType(typeof(ResponseDto[]), StatusCodes.Status200OK)]
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