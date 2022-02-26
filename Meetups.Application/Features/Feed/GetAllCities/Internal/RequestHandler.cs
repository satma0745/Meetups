namespace Meetups.Application.Features.Feed.GetAllCities.Internal;

using System.Threading.Tasks;
using Meetups.Application.Features.Shared.Infrastructure.Internal;
using Meetups.Application.Modules.Persistence;
using Microsoft.EntityFrameworkCore;

public class RequestHandler : RequestHandlerBase<Request, Result, ErrorTypes>
{
    private readonly IApplicationContext context;

    public RequestHandler(IApplicationContext context) =>
        this.context = context;
    
    public override async Task<Response<Result, ErrorTypes>> HandleRequest(Request request)
    {
        var cities = await context.Cities
            .AsNoTracking()
            .ToListAsync();

        var result = new Result(cities);
        return Success(result);
    }
}