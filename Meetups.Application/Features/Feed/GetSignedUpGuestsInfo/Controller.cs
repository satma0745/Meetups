namespace Meetups.Application.Features.Feed.GetSignedUpGuestsInfo;

using System;
using System.Linq;
using System.Threading.Tasks;
using Meetups.Application.Features.Shared.Infrastructure;
using Meetups.Application.Modules.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Tags("Feed")]
public class Controller : ApiControllerBase
{
    private readonly IApplicationContext context;

    public Controller(IApplicationContext context) =>
        this.context = context;

    /// <summary>Get information about all guest who signed up for the meetup.</summary>
    /// <param name="meetupId">Meetup ID.</param>
    /// <response code="200">Guest who signed up for the meetup.</response>
    /// <response code="404">Meetup with the specified ID does not exist.</response>
    [HttpGet("feed/{meetupId:guid}/signed-up-guests")]
    [ProducesResponseType(typeof(ResponseDto[]), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSignedUpGuestsInfo([FromRoute] Guid meetupId)
    {
        var meetup = await context.Meetups
            .AsNoTracking()
            .Include(meetup => meetup.SignedUpGuests)
            .SingleOrDefaultAsync(meetup => meetup.Id == meetupId);
        if (meetup is null)
        {
            return NotFound();
        }

        var response = meetup.SignedUpGuests.Select(Mappings.ToResponseDto);
        return Ok(response);
    }
}