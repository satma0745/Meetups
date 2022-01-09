namespace Meetups.Controllers;

using System;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using Meetups.Context;
using Meetups.DataTransferObjects.User;
using Meetups.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("/api/users")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class UserController : ControllerBase
{
    private readonly Context context;
    private readonly IMapper mapper;

    public UserController(Context context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    /// <summary>Get specific user (with the specified id).</summary>
    /// <param name="id">ID of the user of interest.</param>
    /// <response code="200">User was retrieved successfully.</response>
    /// <response code="404">User with the specified id does not exist.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ReadUserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSpecificUser([FromRoute] Guid id)
    {
        var user = await context.Users.SingleOrDefaultAsync(user => user.Id == id);
        if (user is null)
        {
            return NotFound();
        }

        var readDto = mapper.Map<ReadUserDto>(user);
        return Ok(readDto);
    }

    /// <summary>Get specific user (with the specified username).</summary>
    /// <param name="username">Username of the user of interest.</param>
    /// <response code="200">User was retrieved successfully.</response>
    /// <response code="404">User with the specified username does not exist.</response>
    [HttpGet("{username}")]
    [ProducesResponseType(typeof(ReadUserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSpecificUser([FromRoute] string username)
    {
        var user = await context.Users.SingleOrDefaultAsync(user => user.Username == username);
        if (user is null)
        {
            return NotFound();
        }

        var readDto = mapper.Map<ReadUserDto>(user);
        return Ok(readDto);
    }

    /// <summary>Register a new user.</summary>
    /// <param name="writeDto">DTO to create user from.</param>
    /// <response code="200">New user was created successfully.</response>
    /// <response code="400">Validation failed for DTO.</response>
    /// <response code="409">User with the same username already exists.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterNewUser([FromBody] WriteUserDto writeDto)
    {
        var usernameTaken = await context.Users.AnyAsync(user => user.Username == writeDto.Username);
        if (usernameTaken)
        {
            return Conflict();
        }
        
        var user = mapper.Map<User>(writeDto);
        user.Id = Guid.NewGuid();

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return Ok();
    }
}
