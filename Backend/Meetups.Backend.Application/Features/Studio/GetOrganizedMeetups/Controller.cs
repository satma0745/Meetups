﻿namespace Meetups.Backend.Application.Features.Studio.GetOrganizedMeetups;

using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Meetup.Contract.Models.Enumerations;
using Meetup.Contract.Models.Features.Studio.GetOrganizedMeetups;
using Meetup.Contract.Routing;
using Meetups.Backend.Application.Seedwork;
using Meetups.Backend.Persistence.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Tags(Tags.Studio)]
public class Controller : ApiControllerBase
{
    public Controller(ApplicationContext context, IMapper mapper)
        : base(context, mapper)
    {
    }

    /// <summary>Get a list of all meetups organized by the current user.</summary>
    /// <response code="200">Meetups organized by the current user.</response>
    [Authorize(Roles = UserRoles.Organizer)]
    [HttpGet(Routes.Studio.GetOrganizedMeetups)]
    [ProducesResponseType(typeof(ResponseDto[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrganizedMeetups()
    {
        var organizer = await Context.Organizers
            .AsNoTracking()
            .Include(organizer => organizer.OrganizedMeetups)
            .ThenInclude(meetup => meetup.SignedUpGuests)
            .SingleAsync(organizer => organizer.Id == CurrentUser.UserId);

        var response = Mapper.Map<ICollection<ResponseDto>>(organizer.OrganizedMeetups);
        return Ok(response);
    }
}