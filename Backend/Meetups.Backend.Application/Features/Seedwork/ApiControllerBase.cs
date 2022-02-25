﻿namespace Meetups.Backend.Application.Features.Seedwork;

using System;
using System.Linq;
using System.Net.Mime;
using Meetups.Contract.Models.Tokens;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public abstract class ApiControllerBase : ControllerBase
{
    protected CurrentUserInfo CurrentUser => GetCurrentUserInfo();

    private CurrentUserInfo GetCurrentUserInfo()
    {
        var authenticated = User.Claims.Any();
        if (!authenticated)
        {
            return null;
        }
        
        var userIdClaim = User.Claims.Single(claim => claim.Type == AccessTokenPayload.BearerIdClaim);
        var userId = Guid.Parse(userIdClaim.Value);

        return new CurrentUserInfo(userId);
    }
}