namespace Meetups.Backend.Application.Features.Auth.RegisterNewUser;

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AutoMapper;
using BCrypt.Net;
using Meetup.Contract.Models.Features.Auth.RegisterNewUser;
using Meetup.Contract.Routing;
using Meetups.Backend.Application.Seedwork;
using Meetups.Backend.Persistence.Context;
using Meetups.Backend.Persistence.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserRoles = Meetups.Backend.Persistence.Entities.UserRoles;

[Tags(Tags.Auth)]
public class Controller : ApiControllerBase
{
    public Controller(ApplicationContext context, IMapper mapper)
        : base(context, mapper)
    {
    }
    
    /// <summary>Register a new user.</summary>
    /// <param name="request">DTO to create user from.</param>
    /// <response code="200">New user was created successfully.</response>
    /// <response code="400">Validation failed for DTO.</response>
    /// <response code="409">User with the same username already exists.</response>
    [HttpPost(Routes.Auth.RegisterNewUser)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterNewUser([FromBody] RequestDto request)
    {
        var usernameTaken = await Context.Users.AnyAsync(user => user.Username == request.Username);
        if (usernameTaken)
        {
            return Conflict();
        }
        
        User user = request.AccountType switch
        {
            UserRoles.Guest => Mapper.Map<Guest>(request),
            UserRoles.Organizer => Mapper.Map<Organizer>(request),
            var unmatched => throw new SwitchExpressionException(unmatched)
        };
        user.Id = Guid.NewGuid();
        user.Password = BCrypt.HashPassword(request.Password);

        Context.Users.Add(user);
        await Context.SaveChangesAsync();

        return Ok();
    }
}