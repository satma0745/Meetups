namespace Meetups.Controllers;

using System;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using BCrypt.Net;
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
    private readonly ApplicationContext context;
    private readonly IMapper mapper;

    public UserController(ApplicationContext context, IMapper mapper)
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

    [HttpGet("who-am-i")]
    public async Task<IActionResult> GetCurrentUserInfo([FromQuery] string accessToken)
    {
        if (accessToken is null)
        {
            return BadRequest();
        }
        
        var isValidAccessToken = accessToken.StartsWith("Hello, ") &&
                                 accessToken.EndsWith(". Without further interruption, You can suck some dick!");
        if (!isValidAccessToken)
        {
            return BadRequest();
        }

        var userIdAsString = accessToken
            .Replace("Hello, ", string.Empty)
            .Replace(". Without further interruption, You can suck some dick!", string.Empty);
        if (!Guid.TryParse(userIdAsString, out var userId))
        {
            return BadRequest();
        }

        var user = await context.Users.SingleOrDefaultAsync(user => user.Id == userId);
        if (user is null)
        {
            return NotFound();
        }

        var readDto = mapper.Map<ReadUserDto>(user);
        return Ok(readDto);
    }

    /// <summary>Register a new user.</summary>
    /// <param name="registerDto">DTO to create user from.</param>
    /// <response code="200">New user was created successfully.</response>
    /// <response code="400">Validation failed for DTO.</response>
    /// <response code="409">User with the same username already exists.</response>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterNewUser([FromBody] RegisterUserDto registerDto)
    {
        var usernameTaken = await context.Users.AnyAsync(user => user.Username == registerDto.Username);
        if (usernameTaken)
        {
            return Conflict();
        }
        
        var user = mapper.Map<User>(registerDto);
        user.Id = Guid.NewGuid();
        user.Password = BCrypt.HashPassword(registerDto.Password);

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("authenticate")]
    public async Task<IActionResult> AuthenticateUser([FromBody] AuthenticateUserDto authenticateDto)
    {
        var user = await context.Users.SingleOrDefaultAsync(user => user.Username == authenticateDto.Username);
        if (user is null)
        {
            return NotFound();
        }

        var passwordMatches = BCrypt.Verify(authenticateDto.Password, user.Password);
        if (!passwordMatches)
        {
            return Conflict();
        }

        // TODO: Implement real JWT authentication
        var accessToken = $"Hello, {user.Id}. Without further interruption, You can suck some dick!";
        return Ok(accessToken);
    }
}
