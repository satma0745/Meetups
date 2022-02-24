namespace Meetups.Backend.Application.Features.Seedwork;

using System;
using System.Linq;
using System.Net.Mime;
using Meetup.Contract.Models.Tokens;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public abstract class ApiControllerBase : ControllerBase
{
    protected AccessTokenPayload CurrentUser => GetCurrentUserInfo();

    private AccessTokenPayload GetCurrentUserInfo()
    {
        var authenticated = User.Claims.Any();
        if (!authenticated)
        {
            return null;
        }
        
        var userIdClaim = User.Claims.Single(claim => claim.Type == AccessTokenPayload.UserIdClaim);
        var userRoleClaim = User.Claims.Single(claim => claim.Type == AccessTokenPayload.UserRoleClaim);

        return new AccessTokenPayload
        {
            UserId = Guid.Parse(userIdClaim.Value),
            UserRole = userRoleClaim.Value
        };
    }
}