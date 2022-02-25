namespace Meetups.Backend.Application.Features.Feed.SignUpForMeetup;

using System;
using System.Threading.Tasks;
using Meetups.Backend.Application.Features.Seedwork;
using Meetups.Backend.Application.Modules.Persistence;
using Meetups.Contract.Models.Enumerations;
using Meetups.Contract.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Tags(Tags.Feed)]
public class Controller : ApiControllerBase
{
    private readonly IApplicationContext context;

    public Controller(IApplicationContext context) =>
        this.context = context;

    /// <summary>Sign up for an incoming meetup.</summary>
    /// <param name="meetupId">Meetup ID to sign up for.</param>
    /// <response code="200">Successfully signed up for a meetup.</response>
    /// <response code="404">Meetup with the specified ID does not exist.</response>
    /// <response code="409">You have already signed up for that meetup.</response>
    [Authorize(Roles = UserRoles.Guest)]
    [HttpPost(Routes.Feed.SignUpForMeetup)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> SignUpForMeetup([FromRoute] Guid meetupId)
    {
        var meetup = await context.Meetups
            .AsNoTracking()
            .SingleOrDefaultAsync(meetup => meetup.Id == meetupId);
        if (meetup is null)
        {
            return NotFound();
        }
        
        var currentUser = await context.Guests
            .Include(guest => guest.MeetupsSignedUpTo)
            .SingleAsync(guest => guest.Id == CurrentUser.UserId);
        if (currentUser.IsSignedUpFor(meetup))
        {
            return Conflict();
        }
        
        currentUser.SignUpFor(meetup);
        await context.SaveChangesAsync();

        return Ok();
    }
}