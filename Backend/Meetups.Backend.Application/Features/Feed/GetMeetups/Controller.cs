﻿namespace Meetups.Backend.Application.Features.Feed.GetMeetups;

using System.Linq;
using System.Threading.Tasks;
using Meetup.Contract.Models.Features.Feed.GetMeetups;
using Meetup.Contract.Routing;
using Meetups.Backend.Application.Seedwork;
using Meetups.Backend.Persistence.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Tags(Tags.Feed)]
public class Controller : ApiControllerBase
{
    private readonly ApplicationContext context;

    public Controller(ApplicationContext context) =>
        this.context = context;

    /// <summary>Get several meetups.</summary>
    /// <response code="200">Retrieved meetups successfully.</response>
    /// <response code="400">Validation failed for pagination options.</response>
    [HttpGet(Routes.Feed.GetMeetups)]
    [ProducesResponseType(typeof(ResponseDto[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllMeetups([FromQuery] RequestDto request)
    {
        var meetups = await context.Meetups
            .AsNoTracking()
            .Include(meetup => meetup.SignedUpGuests)
            .ApplySearch($"%{request.Search}%")
            .OrderBy(request.OrderBy)
            .Paginate(request.PageNumber, request.PageSize)
            .ToListAsync();
        
        var response = meetups.Select(Mappings.ToResponseDto);
        return Ok(response);
    }
}