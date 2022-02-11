namespace Meetups.Backend.Core.Seedwork;

using System;
using System.Linq;
using System.Net.Mime;
using AutoMapper;
using Meetup.Contract.Models.Tokens;
using Meetups.Backend.Persistence.Context;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public abstract class ApiControllerBase : ControllerBase
{
    protected ApplicationContext Context { get; }
    protected IMapper Mapper { get; }

    protected AccessTokenPayload CurrentUser => GetCurrentUserInfo();

    protected ApiControllerBase(ApplicationContext context, IMapper mapper)
    {
        Context = context;
        Mapper = mapper;
    }

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