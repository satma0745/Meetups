namespace Meetups.Features.Controllers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using Meetups.Features.DataTransferObjects.Meetup;
using Meetups.Persistence.Context;
using Meetups.Persistence.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("/api/meetups")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class MeetupsController : ControllerBase
{
    private readonly ApplicationContext context;
    private readonly IMapper mapper;

    public MeetupsController(ApplicationContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    /// <summary> Get all meetups. </summary>
    /// <response code="200"> Retrieved meetups successfully. </response>
    [HttpGet]
    [ProducesResponseType(typeof(ReadMeetupDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllMeetups()
    {
        var meetupEntities = await context.Meetups.ToListAsync();
        
        var readDtos = mapper.Map<IEnumerable<ReadMeetupDto>>(meetupEntities);
        return Ok(readDtos);
    }

    /// <summary>Get specific meetup (with the specified id).</summary>
    /// <param name="id">Id of the meetup of interest.</param>
    /// <response code="200">Meetup was retrieved successfully.</response>
    /// <response code="404">Meetup with the specified id does not exist.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ReadMeetupDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSpecificMeetup([FromRoute] Guid id)
    {
        var meetup = await context.Meetups.SingleOrDefaultAsync(meetup => meetup.Id == id);
        if (meetup is null)
        {
            return NotFound();
        }

        var readDto = mapper.Map<ReadMeetupDto>(meetup);
        return Ok(readDto);
    }

    /// <summary>Register new meetup.</summary>
    /// <param name="writeDto">DTO to create meetup from.</param>
    /// <response code="200">New meetup was created successfully.</response>
    /// <response code="400">Validation failed for DTO.</response>
    /// <response code="409">The exact same topic for the meetup has already been taken up.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterNewMeetup([FromBody] WriteMeetupDto writeDto)
    {
        var topicTaken = await context.Meetups.AnyAsync(meetup => meetup.Topic == writeDto.Topic);
        if (topicTaken)
        {
            return Conflict();
        }
        
        var meetupEntity = mapper.Map<Meetup>(writeDto);
        meetupEntity.Id = Guid.NewGuid();
        
        context.Meetups.Add(meetupEntity);
        await context.SaveChangesAsync();
        
        return Ok();
    }

    /// <summary>Updates specific meetup (with the specified id).</summary>
    /// <param name="id">Id of the meetup to be updated.</param>
    /// <param name="writeDto">DTO with updated information about the meetup.</param>
    /// <response code="200">Meetup was updated successfully.</response>
    /// <response code="400">Validation failed for DTO.</response>
    /// <response code="404">Meetup with the specified id does not exist.</response>
    /// <response code="409">The exact same topic for the meetup has already been taken up.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateSpecificMeetup([FromRoute] Guid id, [FromBody] WriteMeetupDto writeDto)
    {
        var topicTaken = await context.Meetups
            .Where(meetup => meetup.Id != id) // search for any other meetups
            .AnyAsync(meetup => meetup.Topic == writeDto.Topic);
        if (topicTaken)
        {
            return Conflict();
        }
        
        var meetup = await context.Meetups.SingleOrDefaultAsync(meetup => meetup.Id == id);
        if (meetup is null)
        {
            return NotFound();
        }

        mapper.Map(writeDto, meetup);
        await context.SaveChangesAsync();

        return Ok();
    }

    /// <summary>Deletes specific meetup (with the specified id).</summary>
    /// <param name="id">Id of the meetup to be deleted.</param>
    /// <response code="200">Meetup was deleted successfully.</response>
    /// <response code="404">Meetup with the specified id does not exist.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSpecificMeetup([FromRoute] Guid id)
    {
        var meetupToDelete = await context.Meetups.SingleOrDefaultAsync(meetup => meetup.Id == id);
        if (meetupToDelete is null)
        {
            return NotFound();
        }

        context.Remove(meetupToDelete);
        await context.SaveChangesAsync();

        return Ok();
    }
}
