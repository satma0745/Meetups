namespace Meetups.Backend.Application.Features.Feed.GetSpecificMeetup;

using System;
using System.Threading.Tasks;
using Meetup.Contract.Models.Features.Feed.GetSpecificMeetup;
using Meetup.Contract.Routing;
using Meetups.Backend.Application.Features.Feed.GetMeetups;
using Meetups.Backend.Application.Modules.Persistence;
using Meetups.Backend.Application.Seedwork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Tags(Tags.Feed)]
public class Controller : ApiControllerBase
{
    private readonly IApplicationContext context;

    public Controller(IApplicationContext context) =>
        this.context = context;

    /// <summary>Get specific meetup (with the specified id).</summary>
    /// <param name="meetupId">Id of the meetup of interest.</param>
    /// <response code="200">Meetup was retrieved successfully.</response>
    /// <response code="404">Meetup with the specified id does not exist.</response>
    [HttpGet(Routes.Feed.GetSpecificMeetup)]
    [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSpecificMeetup([FromRoute] Guid meetupId)
    {
        var meetup = await context.Meetups
            .AsNoTracking()
            .Include(meetup => meetup.SignedUpGuests)
            .Include(meetup => meetup.Place.City)
            .SingleOrDefaultAsync(meetup => meetup.Id == meetupId);
        if (meetup is null)
        {
            return NotFound();
        }

        var response = meetup.ToResponseDto();
        return Ok(response);
    }
}