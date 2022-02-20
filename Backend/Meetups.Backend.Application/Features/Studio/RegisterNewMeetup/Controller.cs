namespace Meetups.Backend.Application.Features.Studio.RegisterNewMeetup;

using System;
using System.Threading.Tasks;
using AutoMapper;
using Meetup.Contract.Models.Features.Studio.RegisterNewMeetup;
using Meetup.Contract.Routing;
using Meetups.Backend.Application.Seedwork;
using Meetups.Backend.Persistence.Context;
using Meetups.Backend.Persistence.Entities;
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

    /// <summary>Register new meetup.</summary>
    /// <param name="request">DTO to create meetup from.</param>
    /// <response code="200">New meetup was created successfully.</response>
    /// <response code="400">Validation failed for DTO.</response>
    /// <response code="409">The exact same topic for the meetup has already been taken up.</response>
    [Authorize(Roles = UserRoles.Organizer)]
    [HttpPost(Routes.Studio.RegisterNewMeetup)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterNewMeetup([FromBody] RequestDto request)
    {
        var topicTaken = await Context.Meetups.AnyAsync(meetup => meetup.Topic == request.Topic);
        if (topicTaken)
        {
            return Conflict();
        }
        
        var organizer = await Context.Organizers
            .Include(organizer => organizer.OrganizedMeetups)
            .SingleAsync(organizer => organizer.Id == CurrentUser.UserId);
        
        var newMeetup = Mapper.Map<Meetup>(request);
        newMeetup.Id = Guid.NewGuid();
        newMeetup.Organizer = organizer;
        
        organizer.OrganizedMeetups.Add(newMeetup);
        
        await Context.SaveChangesAsync();
        
        return Ok();
    }
}