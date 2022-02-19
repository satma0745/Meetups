namespace Meetups.Backend.Application.Features.Studio.UpdateSpecificMeetup;

using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Meetup.Contract.Models.Features.Studio.UpdateSpecificMeetup;
using Meetups.Backend.Application.Seedwork;
using Meetups.Backend.Persistence.Context;
using Meetups.Backend.Persistence.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Tags("Studio")]
public class Controller : ApiControllerBase
{
    public Controller(ApplicationContext context, IMapper mapper)
        : base(context, mapper)
    {
    }

    /// <summary>Updates specific meetup (with the specified id).</summary>
    /// <param name="meetupId">Id of the meetup to be updated.</param>
    /// <param name="request">DTO with updated information about the meetup.</param>
    /// <response code="200">Meetup was updated successfully.</response>
    /// <response code="400">Validation failed for DTO.</response>
    /// <response code="404">Meetup with the specified id does not exist.</response>
    /// <response code="409">The exact same topic for the meetup has already been taken up.</response>
    [Authorize(Roles = UserRoles.Organizer)]
    [HttpPut("studio/{meetupId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateSpecificMeetup([FromRoute] Guid meetupId, [FromBody] RequestDto request)
    {
        var organizer = await Context.Organizers
            .AsNoTracking()
            .Include(organizer => organizer.OrganizedMeetups)
            .SingleAsync(organizer => organizer.Id == CurrentUser.UserId);
        if (organizer.OrganizedMeetups.All(meetup => meetup.Id != meetupId))
        {
            Forbid();
        }
        
        var topicTaken = await Context.Meetups
            .Where(meetup => meetup.Id != meetupId) // exclude the specified meetup (it may preserve it's topic)
            .AnyAsync(meetup => meetup.Topic == request.Topic);
        if (topicTaken)
        {
            return Conflict();
        }
        
        var meetup = await Context.Meetups.SingleOrDefaultAsync(meetup => meetup.Id == meetupId);
        if (meetup is null)
        {
            return NotFound();
        }

        Mapper.Map(request, meetup);
        await Context.SaveChangesAsync();

        return Ok();
    }
}