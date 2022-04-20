namespace Meetups.Application.Features.Studio.RegisterNewCity.Internal;

using System.Threading.Tasks;
using Meetups.Application.Modules.Persistence;
using Meetups.Application.Seedwork.Internal;
using Meetups.Domain.Entities.Meetup;
using Microsoft.EntityFrameworkCore;

public class RequestHandler : RequestHandlerBase<Request, Result, ErrorTypes>
{
    private readonly IApplicationContext context;

    public RequestHandler(IApplicationContext context) =>
        this.context = context;
    
    public override async Task<Response<Result, ErrorTypes>> HandleRequest(Request request)
    {
        var nameTaken = await context.Cities.AnyAsync(city => city.Name == request.Name);
        if (nameTaken)
        {
            return Failure(ErrorTypes.NameAlreadyTaken);
        }

        var city = new City(request.Name);
        context.Cities.Add(city);
        await context.SaveChangesAsync();

        var result = new Result(city.Id, city.Name);
        return Success(result);
    }
}