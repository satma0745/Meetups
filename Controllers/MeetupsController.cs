namespace Meetups.Controllers;

using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Meetups.Context;
using Meetups.DataTransferObjects;
using Meetups.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("/api/meetups")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class MeetupsController : ControllerBase
{
    private readonly MeetupsContext context;

    public MeetupsController(MeetupsContext context) =>
        this.context = context;

    /// <summary> Get all meetups. </summary>
    /// <response code="200"> Retrieved meetups successfully. </response>
    [HttpGet]
    [ProducesResponseType(typeof(ReadMeetupDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllMeetups()
    {
        var meetupEntities = await context.Meetups.ToListAsync();
        
        var readDtos = meetupEntities.ConvertAll(meetupEntity => new ReadMeetupDto
        {
            Id = meetupEntity.Id,
            Topic = meetupEntity.Topic,
            Place = meetupEntity.Place,
            Duration = meetupEntity.Duration,
            StartTime = meetupEntity.StartTime
        });
        return Ok(readDtos);
    }

    /// <summary>Register new meetup.</summary>
    /// <param name="writeDto">DTO to create meetup from.</param>
    /// <response code="200">New meetup was created successfully.</response>
    /// <response code="400">Validation failed for DTO.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterNewMeetup([FromBody] WriteMeetupDto writeDto)
    {
        var meetupEntity = new Meetup
        {
            Id = Guid.NewGuid(),
            Topic = writeDto.Topic,
            Place = writeDto.Place,
            Duration = TimeSpan.FromMinutes(writeDto.Duration),
            StartTime = writeDto.StartTime
        };
        
        context.Meetups.Add(meetupEntity);
        await context.SaveChangesAsync();
        
        return Ok();
    }
}
