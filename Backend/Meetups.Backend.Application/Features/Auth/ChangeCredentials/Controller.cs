﻿namespace Meetups.Backend.Application.Features.Auth.ChangeCredentials;

using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BCrypt.Net;
using Meetup.Contract.Models.Features.Auth.ChangeCredentials;
using Meetup.Contract.Routing;
using Meetups.Backend.Application.Seedwork;
using Meetups.Backend.Persistence.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Tags(Tags.Auth)]
public class Controller : ApiControllerBase
{
    public Controller(ApplicationContext context, IMapper mapper)
        : base(context, mapper)
    {
    }

    /// <summary>Change current user signing credentials.</summary>
    /// <param name="request">New credentials.</param>
    /// <response code="200">Credentials changed successfully.</response>
    /// <response code="409">Username already taken.</response>
    [Authorize]
    [HttpPut(Routes.Auth.ChangeCredentials)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ChangeCredentials([FromBody] RequestDto request)
    {
        var usernameTaken = await Context.Users
            .Include(user => user.RefreshTokens)
            .Where(user => user.Id != CurrentUser.UserId)
            .AnyAsync(user => user.Username == request.Username);
        if (usernameTaken)
        {
            return Conflict();
        }
        
        var user = await Context.Users.SingleAsync(user => user.Id == CurrentUser.UserId);
        user.ChangeCredentials(request.Username, BCrypt.HashPassword(request.Password));
        user.RevokeAllRefreshTokens();
        await Context.SaveChangesAsync();
        
        return Ok();
    }
}