namespace Meetups.Features.Meetup.UpdateSpecificMeetup;

using System;
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

    /// <summary>Updates specific meetup (with the specified id).</summary>
    /// <param name="id">Id of the meetup to be updated.</param>
    /// <param name="dto">DTO with updated information about the meetup.</param>
    /// <response code="200">Meetup was updated successfully.</response>
    /// <response code="400">Validation failed for DTO.</response>
    /// <response code="404">Meetup with the specified id does not exist.</response>
    /// <response code="409">The exact same topic for the meetup has already been taken up.</response>
    [HttpPut("meetups/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateSpecificMeetup([FromRoute] Guid id, [FromBody] RequestDto dto)
    {
        var topicTaken = await Context.Meetups
            .Where(meetup => meetup.Id != id) // exclude the specified meetup (it may preserve it's topic)
            .AnyAsync(meetup => meetup.Topic == dto.Topic);
        if (topicTaken)
        {
            return Conflict();
        }
        
        var meetup = await Context.Meetups.SingleOrDefaultAsync(meetup => meetup.Id == id);
        if (meetup is null)
        {
            return NotFound();
        }

        Mapper.Map(dto, meetup);
        await Context.SaveChangesAsync();

        return Ok();
    }
}