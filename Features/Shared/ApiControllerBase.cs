namespace Meetups.Features.Shared;

using System;
using System.Linq;
using System.Net.Mime;
using AutoMapper;
using Meetups.Persistence.Context;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public abstract class ApiControllerBase : ControllerBase
{
    protected ApplicationContext Context { get; }
    protected IMapper Mapper { get; }

    protected CurrentUserInfo CurrentUser => GetCurrentUserInfo();

    protected ApiControllerBase(ApplicationContext context, IMapper mapper)
    {
        Context = context;
        Mapper = mapper;
    }

    private CurrentUserInfo GetCurrentUserInfo()
    {
        var userIdClaim = User.Claims.SingleOrDefault(claim => claim.Type == "sub");
        if (userIdClaim is null)
        {
            return null;
        }
        
        var userId = Guid.Parse(userIdClaim.Value);
        return new CurrentUserInfo { Id = userId };
    }
    
    protected class CurrentUserInfo
    {
        public Guid Id { get; init; }
    }
}