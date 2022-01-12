namespace Meetups.Features.User.GetSpecificUser;

using System;
using System.Threading.Tasks;
using AutoMapper;
using Meetups.Features.Shared;
using Meetups.Persistence.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Tags("User")]
public class Controller : ApiControllerBase
{
    public Controller(ApplicationContext context, IMapper mapper)
        : base(context, mapper)
    {
    }
    
    /// <summary>Get specific user (with the specified id).</summary>
    /// <param name="id">ID of the user of interest.</param>
    /// <response code="200">User was retrieved successfully.</response>
    /// <response code="404">User with the specified id does not exist.</response>
    [HttpGet("users/{id:guid}")]
    [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSpecificUser([FromRoute] Guid id)
    {
        var user = await Context.Users.SingleOrDefaultAsync(user => user.Id == id);
        if (user is null)
        {
            return NotFound();
        }

        var dto = Mapper.Map<ResponseDto>(user);
        return Ok(dto);
    }

    /// <summary>Get specific user (with the specified username).</summary>
    /// <param name="username">Username of the user of interest.</param>
    /// <response code="200">User was retrieved successfully.</response>
    /// <response code="404">User with the specified username does not exist.</response>
    [HttpGet("users/{username}")]
    [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSpecificUser([FromRoute] string username)
    {
        var user = await Context.Users.SingleOrDefaultAsync(user => user.Username == username);
        if (user is null)
        {
            return NotFound();
        }

        var dto = Mapper.Map<ResponseDto>(user);
        return Ok(dto);
    }
}