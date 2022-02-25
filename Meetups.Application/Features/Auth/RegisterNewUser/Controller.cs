namespace Meetups.Application.Features.Auth.RegisterNewUser;

using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using BCrypt.Net;
using Meetups.Application.Features.Shared.Contracts.Auth;
using Meetups.Application.Features.Shared.Infrastructure;
using Meetups.Application.Modules.Persistence;
using Meetups.Domain.Entities.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Tags("Auth")]
public class Controller : ApiControllerBase
{
    private readonly IApplicationContext context;

    public Controller(IApplicationContext context) =>
        this.context = context;
    
    /// <summary>Register a new user.</summary>
    /// <param name="request">DTO to create user from.</param>
    /// <response code="200">New user was created successfully.</response>
    /// <response code="400">Validation failed for DTO.</response>
    /// <response code="409">User with the same username already exists.</response>
    [HttpPost("auth/register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterNewUser([FromBody] RequestDto request)
    {
        var usernameTaken = await context.Users.AnyAsync(user => user.Username == request.Username);
        if (usernameTaken)
        {
            return Conflict();
        }

        var password = BCrypt.HashPassword(request.Password);
        User user = request.AccountType switch
        {
            UserRoles.Guest => new Guest(request.Username, password, request.DisplayName),
            UserRoles.Organizer => new Organizer(request.Username, password, request.DisplayName),
            var unmatched => throw new SwitchExpressionException(unmatched)
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return Ok();
    }
}