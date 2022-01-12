namespace Meetups.Features.Shared;

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

    protected ApiControllerBase(ApplicationContext context, IMapper mapper)
    {
        Context = context;
        Mapper = mapper;
    }
}