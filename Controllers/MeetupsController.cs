namespace Meetups.Controllers;

using System;
using System.Threading.Tasks;
using Meetups.Context;
using Meetups.DataTransferObjects;
using Meetups.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("/api/meetups")]
public class MeetupsController : ControllerBase
{
    private readonly MeetupsContext context;

    public MeetupsController(MeetupsContext context) =>
        this.context = context;

    [HttpGet]
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

    /// <example>
    /// {
    ///   "topic": "Microsoft naming issues.",
    ///   "place": "Oslo",
    ///   "duration": 180,
    ///   "startTime": "2022-01-09T12:00:00Z"
    /// }
    /// </example>
    [HttpPost]
    public async Task<IActionResult> RegisterNewMeetup(CreateMeetupDto createDto)
    {
        var meetupEntity = new Meetup
        {
            Id = Guid.NewGuid(),
            Topic = createDto.Topic,
            Place = createDto.Place,
            Duration = TimeSpan.FromMinutes(createDto.Duration),
            StartTime = createDto.StartTime
        };
        
        context.Meetups.Add(meetupEntity);
        await context.SaveChangesAsync();
        
        return Ok();
    }
}
