namespace Meetups.Application.Features.Studio.RegisterNewCity;

using System.Threading.Tasks;
using Meetups.Application.Features.Shared.Contracts.Auth;
using Meetups.Application.Features.Shared.Infrastructure;
using Meetups.Application.Modules.Persistence;
using Meetups.Domain.Entities.Meetup;
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

    /// <summary>Register a new city.</summary>
    /// <param name="request">DTO to create city based on.</param>
    /// <response code="200">New city was registered successfully.</response>
    /// <response code="409">City with te same name already exists.</response>
    [Authorize(Roles = UserRoles.Organizer)]
    [HttpPost("studio/new-city")]
    [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterNewCity([FromBody] RequestDto request)
    {
        var nameTaken = await context.Cities.AnyAsync(city => city.Name == request.Name);
        if (nameTaken)
        {
            return Conflict();
        }

        var city = new City(request.Name);
        context.Cities.Add(city);
        await context.SaveChangesAsync();

        return Ok(city);
    }
}