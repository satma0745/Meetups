namespace Meetups.Backend.Application.Features.Studio.UpdateSpecificMeetup;

using System;
using System.Linq;
using System.Threading.Tasks;
using Meetup.Contract.Models.Enumerations;
using Meetup.Contract.Models.Features.Studio.UpdateSpecificMeetup;
using Meetup.Contract.Routing;
using Meetups.Backend.Application.Seedwork;
using Meetups.Backend.Entities.Meetup;
using Meetups.Backend.Persistence.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Tags(Tags.Studio)]
public class Controller : ApiControllerBase
{
    private readonly ApplicationContext context;

    public Controller(ApplicationContext context) =>
        this.context = context;

    /// <summary>Updates specific meetup (with the specified id).</summary>
    /// <param name="meetupId">Id of the meetup to be updated.</param>
    /// <param name="request">DTO with updated information about the meetup.</param>
    /// <response code="200">Meetup was updated successfully.</response>
    /// <response code="400">Validation failed for DTO.</response>
    /// <response code="404">Meetup with the specified id does not exist.</response>
    /// <response code="409">The exact same topic for the meetup has already been taken up.</response>
    [Authorize(Roles = UserRoles.Organizer)]
    [HttpPut(Routes.Studio.UpdateSpecificMeetup)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateSpecificMeetup([FromRoute] Guid meetupId, [FromBody] RequestDto request)
    {
        var topicTaken = await context.Meetups
            .Where(meetup => meetup.Id != meetupId) // exclude the specified meetup (it may preserve it's topic)
            .AnyAsync(meetup => meetup.Topic == request.Topic);
        if (topicTaken)
        {
            return Conflict();
        }
        
        var meetup = await context.Meetups
            .Include(meetup => meetup.Organizer)
            .SingleOrDefaultAsync(meetup => meetup.Id == meetupId);
        if (meetup is null)
        {
            return NotFound();
        }
        if (meetup.Organizer.Id != CurrentUser.UserId)
        {
            return Forbid();
        }

        var duration = new MeetupDuration(request.Duration.Hours, request.Duration.Minutes);
        meetup.UpdateMeetupInfo(request.Topic, request.Place, duration, request.StartTime);
        await context.SaveChangesAsync();

        return Ok();
    }
}