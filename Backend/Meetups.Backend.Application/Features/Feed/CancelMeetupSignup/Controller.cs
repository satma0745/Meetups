namespace Meetups.Backend.Application.Features.Feed.CancelMeetupSignup;

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

    /// <summary>Cancel meetup signup.</summary>
    /// <param name="meetupId">Id of the meetup to cancel signup for.</param>
    /// <response code="200">Signup for a meetup canceled successfully.</response>
    /// <response code="404">Meetup with the specified id does not exist.</response>
    /// <response code="409">User has to sign up for the specified meetup in the first place.</response>
    [Authorize(Roles = UserRoles.Guest)]
    [HttpPost(Routes.Feed.CancelMeetupSignup)]
    public async Task<IActionResult> CancelMeetupSignup([FromRoute] Guid meetupId)
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
        if (!currentUser.IsSignedUpFor(meetup))
        {
            return Conflict();
        }
        
        currentUser.CancelSignUpFor(meetup);
        await context.SaveChangesAsync();

        return Ok();
    }
}