namespace Meetups.Controllers;

using System;
using System.Threading.Tasks;
using Meetups.Context;
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
        var allMeetups = await context.Meetups.ToListAsync();
        return Ok(allMeetups);
    }

    /// <example>
    /// {
    ///   "topic": "Microsoft naming issues.",
    ///   "place": "Oslo",
    ///   "duration": 180,
    ///   "startTime": "2022-01-09T12:00:00.000Z"
    /// }
    /// </example>
    [HttpPost]
    public async Task<IActionResult> RegisterNewMeetup(Meetup meetup)
    {
        // set id if requester did not bother to
        if (meetup.Id == Guid.Empty)
        {
            meetup.Id = Guid.NewGuid();
        }
        
        context.Meetups.Add(meetup);
        await context.SaveChangesAsync();
        
        return Ok();
    }
}
