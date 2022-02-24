namespace Meetups.Backend.Application.Features.Studio.UpdateMeetupDescription;

using System;
using System.Threading.Tasks;
using Meetup.Contract.Models.Enumerations;
using Meetup.Contract.Models.Features.Studio.UpdateMeetupDescription;
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

    /// <summary>Updates description of a specific meetup (with the specified ID).</summary>
    /// <param name="meetupId">Meetup ID to update description of.</param>
    /// <param name="request">DTO with the updated meetup description.</param>
    /// <response code="200">Meetup description was updated successfully.</response>
    /// <response code="403">Only the organizer of a meetup can update it's description.</response>
    /// <response code="404">Specified meetup does not exist.</response>
    [Authorize(Roles = UserRoles.Organizer)]
    [HttpPut(Routes.Studio.UpdateMeetupDescription)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RescheduleMeetup([FromRoute] Guid meetupId, [FromBody] RequestDto request)
    {
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

        meetup.UpdateDescription(request.Topic);
        await context.SaveChangesAsync();

        return Ok();
    }
}