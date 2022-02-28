namespace Meetups.Application.Features.Feed.GetAllCities.Internal;

using System.Linq;
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
        var cities = await context.Cities.ToListAsync();

        var cityModels = cities.Select(city => new CityModel(city.Id, city.Name));
        var result = new Result(cityModels);
        return Success(result);
    }
}