namespace Meetups.Features.Feed.SignUpForMeetup;

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

[Tags("Feed")]
public class Controller : ApiControllerBase
{
    public Controller(ApplicationContext context, IMapper mapper)
        : base(context, mapper)
    {
    }

    /// <summary>Sign up for an incoming meetup.</summary>
    /// <param name="meetupId">Meetup ID to sign up for.</param>
    /// <response code="200">Successfully signed up for a meetup.</response>
    /// <response code="403">Only guests can sign up for meetups.</response>
    /// <response code="404">Meetup with the specified ID does not exist.</response>
    /// <response code="409">You have already signed up for that meetup.</response>
    [Authorize]
    [HttpPost("feed/{meetupId:guid}/sign-up")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> SignUpForMeetup([FromRoute] Guid meetupId)
    {
        var currentUser = await Context.Users
            .AsNoTracking()
            .SingleAsync(user => user.Id == CurrentUser.Id);
        if (currentUser is not Guest guest)
        {
            return Forbid();
        }
        
        var meetup = await Context.Meetups
            .Include(meetup => meetup.SignedUpGuests)
            .SingleOrDefaultAsync(meetup => meetup.Id == meetupId);
        if (meetup is null)
        {
            return NotFound();
        }

        var alreadySignedUp = meetup.SignedUpGuests.Any(signedUpGuest => signedUpGuest.Id == guest.Id);
        if (alreadySignedUp)
        {
            return Conflict();
        }
        
        meetup.SignedUpGuests.Add(guest);
        await Context.SaveChangesAsync();

        return Ok();
    }
}