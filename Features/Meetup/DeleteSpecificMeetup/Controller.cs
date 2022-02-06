namespace Meetups.Features.Meetup.DeleteSpecificMeetup;

using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Meetups.Features.Shared;
using Meetups.Persistence.Context;
using Meetups.Persistence.Entities;
using Microsoft.AspNetCore.Authorization;
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

    /// <summary>Deletes specific meetup (with the specified id).</summary>
    /// <param name="id">Id of the meetup to be deleted.</param>
    /// <response code="200">Meetup was deleted successfully.</response>
    /// <response code="403">Only meetup organizer can delete it's own meetups.</response>
    /// <response code="404">Meetup with the specified id does not exist.</response>
    [Authorize]
    [HttpDelete("meetups/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSpecificMeetup([FromRoute] Guid id)
    {
        var meetupToDelete = await Context.Meetups
            .AsNoTracking()
            .SingleOrDefaultAsync(meetup => meetup.Id == id);
        if (meetupToDelete is null)
        {
            return NotFound();
        }
        
        var currentUser = await Context.Users
            .AsNoTracking()
            .Include(user => (user as Organizer).OrganizedMeetups)
            .SingleAsync(user => user.Id == CurrentUser.Id);
        if (currentUser is not Organizer organizer ||
            organizer.OrganizedMeetups.All(organizedMeetup => organizedMeetup.Id != id))
        {
            return Forbid();
        }

        Context.Remove(meetupToDelete);
        await Context.SaveChangesAsync();

        return Ok();
    }
}