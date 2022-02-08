namespace Meetups.Backend.Features.Feed.GetSpecificMeetup;

using System;
using System.Threading.Tasks;
using AutoMapper;
using Meetups.Backend.Features.Shared;
using Meetups.Backend.Persistence.Context;
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

    /// <summary>Get specific meetup (with the specified id).</summary>
    /// <param name="meetupId">Id of the meetup of interest.</param>
    /// <response code="200">Meetup was retrieved successfully.</response>
    /// <response code="404">Meetup with the specified id does not exist.</response>
    [HttpGet("feed/{meetupId:guid}")]
    [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSpecificMeetup([FromRoute] Guid meetupId)
    {
        var meetup = await Context.Meetups
            .AsNoTracking()
            .Include(meetup => meetup.SignedUpGuests)
            .SingleOrDefaultAsync(meetup => meetup.Id == meetupId);
        if (meetup is null)
        {
            return NotFound();
        }

        var dto = Mapper.Map<ResponseDto>(meetup);
        return Ok(dto);
    }
}