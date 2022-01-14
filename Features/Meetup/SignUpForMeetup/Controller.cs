namespace Meetups.Features.Meetup.SignUpForMeetup;

using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Meetups.Features.Shared;
using Meetups.Persistence.Context;
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

    /// <summary>Sign up for an incoming meetup.</summary>
    /// <param name="meetupId">Meetup ID to sign up for.</param>
    /// <response code="200">Successfully signed up for a meetup.</response>
    /// <response code="404">Meetup with the specified ID does not exist.</response>
    /// <response code="409">You have already signed up for that meetup.</response>
    [Authorize]
    [HttpPost("meetups/{meetupId}/sign-up")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> SignUpForMeetup([FromRoute] Guid meetupId)
    {
        var meetup = await Context.Meetups
            .AsNoTracking()
            .SingleOrDefaultAsync(meetup => meetup.Id == meetupId);
        if (meetup is null)
        {
            return NotFound();
        }

        var currentUser = await Context.Users
            .Include(user => user.MeetupsSignedUpTo)
            .SingleAsync(user => user.Id == CurrentUser.Id);

        var alreadySignedUp = currentUser.MeetupsSignedUpTo.Any(meetup => meetup.Id == meetupId);
        if (alreadySignedUp)
        {
            return Conflict();
        }
        
        currentUser.MeetupsSignedUpTo.Add(meetup);
        await Context.SaveChangesAsync();

        return Ok();
    }
}