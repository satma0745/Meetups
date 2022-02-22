namespace Meetups.Backend.Application.Features.Studio.RegisterNewMeetup;

using System.Threading.Tasks;
using Meetup.Contract.Models.Enumerations;
using Meetup.Contract.Models.Features.Studio.RegisterNewMeetup;
using Meetup.Contract.Routing;
using Meetups.Backend.Application.Seedwork;
using Meetups.Backend.Entities.Meetup;
using Meetups.Backend.Persistence.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Tags(Tags.Studio)]
public class Controller : ApiControllerBase
{
    private readonly ApplicationContext context;

    public Controller(ApplicationContext context) =>
        this.context = context;

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
        var topicTaken = await context.Meetups.AnyAsync(meetup => meetup.Topic == request.Topic);
        if (topicTaken)
        {
            return Conflict();
        }
        
        var organizer = await context.Organizers
            .Include(organizer => organizer.OrganizedMeetups)
            .SingleAsync(organizer => organizer.Id == CurrentUser.UserId);

        var duration = new MeetupDuration(request.Duration.Hours, request.Duration.Minutes);
        var meetup = new Meetup(request.Topic, request.Place, duration, request.StartTime);
        
        organizer.AddOrganizedMeetup(meetup);
        await context.SaveChangesAsync();
        
        return Ok();
    }
}