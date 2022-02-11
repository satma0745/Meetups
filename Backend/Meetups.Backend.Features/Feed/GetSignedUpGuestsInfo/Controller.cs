namespace Meetups.Backend.Features.Feed.GetSignedUpGuestsInfo;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Meetup.Contract.Models.Features.Feed.GetSignedUpGuestsInfo;
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

    /// <summary>Get information about all guest who signed up for the meetup.</summary>
    /// <param name="meetupId">Meetup ID.</param>
    /// <response code="200">Guest who signed up for the meetup.</response>
    /// <response code="404">Meetup with the specified ID does not exist.</response>
    [HttpGet("feed/{meetupId:guid}/signed-up-guests")]
    [ProducesResponseType(typeof(ResponseDto[]), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSignedUpGuestsInfo([FromRoute] Guid meetupId)
    {
        var meetup = await Context.Meetups
            .AsNoTracking()
            .Include(meetup => meetup.SignedUpGuests)
            .SingleOrDefaultAsync(meetup => meetup.Id == meetupId);
        if (meetup is null)
        {
            return NotFound();
        }

        var response = Mapper.Map<ICollection<ResponseDto>>(meetup.SignedUpGuests);
        return Ok(response);
    }
}