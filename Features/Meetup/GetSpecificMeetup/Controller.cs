namespace Meetups.Features.Meetup.GetSpecificMeetup;

using System;
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

    /// <summary>Get specific meetup (with the specified id).</summary>
    /// <param name="id">Id of the meetup of interest.</param>
    /// <response code="200">Meetup was retrieved successfully.</response>
    /// <response code="404">Meetup with the specified id does not exist.</response>
    [HttpGet("meetups/{id:guid}")]
    [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSpecificMeetup([FromRoute] Guid id)
    {
        var meetup = await Context.Meetups
            .AsNoTracking()
            .Include(meetup => meetup.SignedUpGuests)
            .SingleOrDefaultAsync(meetup => meetup.Id == id);
        if (meetup is null)
        {
            return NotFound();
        }

        var dto = Mapper.Map<ResponseDto>(meetup);
        return Ok(dto);
    }
}