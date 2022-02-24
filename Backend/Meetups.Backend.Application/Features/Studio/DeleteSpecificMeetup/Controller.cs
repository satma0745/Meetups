namespace Meetups.Backend.Application.Features.Studio.DeleteSpecificMeetup;

using System;
using System.Linq;
using System.Threading.Tasks;
using Meetup.Contract.Models.Enumerations;
using Meetup.Contract.Routing;
using Meetups.Backend.Application.Features.Seedwork;
using Meetups.Backend.Application.Modules.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Tags(Tags.Studio)]
public class Controller : ApiControllerBase
{
    private readonly IApplicationContext context;

    public Controller(IApplicationContext context) =>
        this.context = context;

    /// <summary>Deletes specific meetup (with the specified id).</summary>
    /// <param name="meetupId">Id of the meetup to be deleted.</param>
    /// <response code="200">Meetup was deleted successfully.</response>
    /// <response code="404">Meetup with the specified id does not exist.</response>
    [Authorize(Roles = UserRoles.Organizer)]
    [HttpDelete(Routes.Studio.DeleteSpecificMeetup)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSpecificMeetup([FromRoute] Guid meetupId)
    {
        var meetupToDelete = await context.Meetups
            .AsNoTracking()
            .SingleOrDefaultAsync(meetup => meetup.Id == meetupId);
        if (meetupToDelete is null)
        {
            return NotFound();
        }
        
        var organizer = await context.Organizers
            .AsNoTracking()
            .Include(organizer => organizer.OrganizedMeetups)
            .SingleAsync(organizer => organizer.Id == CurrentUser.UserId);
        if (organizer.OrganizedMeetups.All(organizedMeetup => organizedMeetup.Id != meetupId))
        {
            return Forbid();
        }

        context.Meetups.Remove(meetupToDelete);
        await context.SaveChangesAsync();

        return Ok();
    }
}