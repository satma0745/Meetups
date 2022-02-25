namespace Meetups.Application.Features.Studio.RescheduleMeetup;

using System;
using System.Threading.Tasks;
using Meetups.Application.Features.Shared.Contracts;
using Meetups.Application.Features.Shared.Contracts.Auth;
using Meetups.Application.Features.Shared.Infrastructure;
using Meetups.Application.Modules.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Tags("Studio")]
public class Controller : ApiControllerBase
{
    private readonly IApplicationContext context;

    public Controller(IApplicationContext context) =>
        this.context = context;

    /// <summary>Reschedules specific meetup (with the specified ID).</summary>
    /// <param name="meetupId">Meetup ID to reschedule.</param>
    /// <param name="request">DTO with the updated scheduling information.</param>
    /// <response code="200">Meetup was rescheduled successfully.</response>
    /// <response code="403">Only the organizer of a meetup can reschedule a specific meetup.</response>
    /// <response code="404">Specified meetup or city does not exist.</response>
    [Authorize(Roles = UserRoles.Organizer)]
    [HttpPost("studio/{meetupId:guid}/reschedule")]
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

        var city = await context.Cities.SingleOrDefaultAsync(city => city.Id == request.Place.CityId);
        if (city is null)
        {
            return NotFound();
        }

        meetup.Reschedule(
            place: request.Place.ToMeetupPlace(city),
            duration: request.Duration.ToMeetupDuration(),
            startTime: request.StartTime);
        await context.SaveChangesAsync();

        return Ok();
    }
}