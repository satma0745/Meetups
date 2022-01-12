namespace Meetups.Features.Meetup.GetAllMeetups;

using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Meetups.Features.Shared;
using Meetups.Persistence.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Tags("Meetup")]
public class Controller : ApiControllerBase
{
    public Controller(ApplicationContext context, IMapper mapper)
        : base(context, mapper)
    {
    }

    /// <summary> Get all meetups. </summary>
    /// <response code="200"> Retrieved meetups successfully. </response>
    [HttpGet("meetups")]
    [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllMeetups()
    {
        var meetups = await Context.Meetups.ToListAsync();
        var dtos = Mapper.Map<IEnumerable<ResponseDto>>(meetups);
        return Ok(dtos);
    }
}