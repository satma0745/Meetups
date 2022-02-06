namespace Meetups.Features.Meetup.RegisterNewMeetup;

using System;
using System.Threading.Tasks;
using AutoMapper;
using Meetups.Features.Shared;
using Meetups.Persistence.Context;
using Meetups.Persistence.Entities;
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

    /// <summary>Register new meetup.</summary>
    /// <param name="request">DTO to create meetup from.</param>
    /// <response code="200">New meetup was created successfully.</response>
    /// <response code="400">Validation failed for DTO.</response>
    /// <response code="403">Only meetup organizers can register new meetups.</response>
    /// <response code="409">The exact same topic for the meetup has already been taken up.</response>
    [Authorize]
    [HttpPost("meetups")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterNewMeetup([FromBody] RequestDto request)
    {
        var currentUser = await Context.Users.SingleAsync(user => user.Id == CurrentUser.Id);
        if (currentUser is not Organizer organizer)
        {
            return Forbid();
        }
        
        var topicTaken = await Context.Meetups.AnyAsync(meetup => meetup.Topic == request.Topic);
        if (topicTaken)
        {
            return Conflict();
        }
        
        var meetup = Mapper.Map<Meetup>(request);
        meetup.Id = Guid.NewGuid();
        meetup.Organizer = organizer;
        
        Context.Meetups.Add(meetup);
        await Context.SaveChangesAsync();
        
        return Ok();
    }
}