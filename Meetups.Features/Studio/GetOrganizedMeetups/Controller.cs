﻿namespace Meetups.Features.Studio.GetOrganizedMeetups;

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
    [Authorize(Roles = UserRoles.Organizer)]
    [HttpGet("studio/organized")]
    [ProducesResponseType(typeof(OrganizedMeetupDto[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrganizedMeetups()
    {
        var organizer = await Context.Organizers
            .AsNoTracking()
            .Include(organizer => organizer.OrganizedMeetups)
            .ThenInclude(meetup => meetup.SignedUpGuests)
            .SingleAsync(organizer => organizer.Id == CurrentUser.Id);

        var organizedMeetups = Mapper.Map<ICollection<OrganizedMeetupDto>>(organizer.OrganizedMeetups);
        return Ok(organizedMeetups);
    }
}