namespace Meetups.Features.Studio.GetOrganizedMeetups;

using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Meetups.Features.Shared;
using Meetups.Persistence.Context;
using Meetups.Persistence.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Tags("Studio")]
public class Controller : ApiControllerBase
{
    public Controller(ApplicationContext context, IMapper mapper)
        : base(context, mapper)
    {
    }

    /// <summary>Get a list of all meetups organized by the current user.</summary>
    /// <response code="200">Meetups organized by the current user.</response>
    /// <response code="409">Only users with an "organizer" type account can organize meetups.</response>
    [Authorize]
    [HttpGet("studio/organized")]
    [ProducesResponseType(typeof(OrganizedMeetupDto[]), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> GetOrganizedMeetups()
    {
        var currentUser = await Context.Users
            .AsNoTracking()
            .Include(user => (user as Organizer).OrganizedMeetups)
            .ThenInclude(meetup => meetup.SignedUpGuests)
            .SingleAsync(user => user.Id == CurrentUser.Id);
        if (currentUser is not Organizer organizer)
        {
            return Conflict();
        }

        var organizedMeetups = Mapper.Map<ICollection<OrganizedMeetupDto>>(organizer.OrganizedMeetups);
        return Ok(organizedMeetups);
    }
}