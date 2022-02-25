namespace Meetups.Application.Features.Studio.GetOrganizedMeetups;

using System.Linq;
using System.Threading.Tasks;
using Meetups.Application.Features.Shared.Contracts.Auth;
using Meetups.Application.Features.Shared.Infrastructure;
using Meetups.Application.Modules.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Tags("Studio")]
public class Controller : ApiControllerBase
{
    private readonly IApplicationContext context;

    public Controller(IApplicationContext context) =>
        this.context = context;

    /// <summary>Get a list of all meetups organized by the current user.</summary>
    /// <response code="200">Meetups organized by the current user.</response>
    [Authorize(Roles = UserRoles.Organizer)]
    [HttpGet("studio/organized")]
    [ProducesResponseType(typeof(ResponseDto[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrganizedMeetups()
    {
        var organizer = await context.Organizers
            .AsNoTracking()
            .Include(organizer => organizer.OrganizedMeetups)
            .ThenInclude(meetup => meetup.SignedUpGuests)
            .Include(organizer => organizer.OrganizedMeetups)
            .ThenInclude(meetup => meetup.Place.City)
            .SingleAsync(organizer => organizer.Id == CurrentUser.UserId);

        var response = organizer.OrganizedMeetups.Select(Mappings.ToResponseDto);
        return Ok(response);
    }
}