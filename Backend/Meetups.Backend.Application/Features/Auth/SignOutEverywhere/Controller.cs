﻿namespace Meetups.Backend.Application.Features.Auth.SignOutEverywhere;

using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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

    /// <summary>Sign out user everywhere.</summary>
    /// <response code="200">User signed out successfully.</response>
    [Authorize]
    [HttpPost(Routes.Auth.SignOutEverywhere)]
    public async Task<IActionResult> SignOutEverywhere()
    {
        var userRefreshTokens = await Context.RefreshTokens
            .AsNoTracking()
            .Where(token => token.BearerId == CurrentUser.UserId)
            .ToListAsync();
        
        Context.RefreshTokens.RemoveRange(userRefreshTokens);
        await Context.SaveChangesAsync();

        return Ok();
    }
}