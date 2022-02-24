﻿namespace Meetups.Backend.Application.Features.Studio.RegisterNewMeetup;

using System.Threading.Tasks;
using Meetup.Contract.Models.Enumerations;
using Meetup.Contract.Models.Features.Studio.RegisterNewMeetup;
using Meetup.Contract.Routing;
using Meetups.Backend.Application.Features.Seedwork;
using Meetups.Backend.Application.Modules.Persistence;
using Meetups.Backend.Domain.Entities.Meetup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Tags(Tags.Studio)]
public class Controller : ApiControllerBase
{
    private readonly IApplicationContext context;

    public Controller(IApplicationContext context) =>
        this.context = context;

    /// <summary>Register new meetup.</summary>
    /// <param name="request">DTO to create meetup from.</param>
    /// <response code="200">New meetup was created successfully.</response>
    /// <response code="400">Validation failed for DTO.</response>
    /// <response code="404">Specified city does not exist.</response>
    /// <response code="409">The exact same topic for the meetup has already been taken up.</response>
    [Authorize(Roles = UserRoles.Organizer)]
    [HttpPost(Routes.Studio.RegisterNewMeetup)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterNewMeetup([FromBody] RequestDto request)
    {
        var topicTaken = await context.Meetups.AnyAsync(meetup => meetup.Topic == request.Topic);
        if (topicTaken)
        {
            return Conflict();
        }

        var city = await context.Cities.SingleOrDefaultAsync(city => city.Id == request.Place.CityId);
        if (city is null)
        {
            return NotFound();
        }

        var organizer = await context.Organizers
            .Include(organizer => organizer.OrganizedMeetups)
            .ThenInclude(meetup => meetup.Place.City)
            .SingleAsync(organizer => organizer.Id == CurrentUser.UserId);

        var meetup = new Meetup(
            topic: request.Topic,
            place: request.Place.ToMeetupPlace(city),
            duration: request.Duration.ToMeetupDuration(),
            startTime: request.StartTime);
        organizer.AddOrganizedMeetup(meetup);
        
        await context.SaveChangesAsync();
        
        return Ok();
    }
}