namespace Meetups.Controllers;

using System;
using System.Collections.Generic;
using Meetups.Entities;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/meetups")]
public class MeetupsController : ControllerBase
{
    private static readonly List<Meetup> Meetups = new();

    [HttpGet]
    public IActionResult GetAllMeetups() => Ok(Meetups);

    /// <example>
    /// {
    ///   "topic": "Microsoft naming issues.",
    ///   "place": "Oslo",
    ///   "duration": 180,
    ///   "startTime": "2022-01-09T12:00:00.000Z"
    /// }
    /// </example>
    [HttpPost]
    public IActionResult RegisterNewMeetup(Meetup meetup)
    {
        // set id if requester did not bother to
        if (meetup.Id == Guid.Empty)
        {
            meetup.Id = Guid.NewGuid();
        }
        
        Meetups.Add(meetup);
        return Ok();
    }
}
