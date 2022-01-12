namespace Meetups.Features.Meetup.RegisterNewMeetup;

using System;
using System.Threading.Tasks;
using AutoMapper;
using Meetups.Features.Shared;
using Meetups.Persistence.Context;
using Meetups.Persistence.Entities;
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
    /// <param name="dto">DTO to create meetup from.</param>
    /// <response code="200">New meetup was created successfully.</response>
    /// <response code="400">Validation failed for DTO.</response>
    /// <response code="409">The exact same topic for the meetup has already been taken up.</response>
    [HttpPost("meetups")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterNewMeetup([FromBody] RequestDto dto)
    {
        var topicTaken = await Context.Meetups.AnyAsync(meetup => meetup.Topic == dto.Topic);
        if (topicTaken)
        {
            return Conflict();
        }
        
        var meetup = Mapper.Map<Meetup>(dto);
        meetup.Id = Guid.NewGuid();
        
        Context.Meetups.Add(meetup);
        await Context.SaveChangesAsync();
        
        return Ok();
    }
}