namespace Meetups.Application.Features.Studio.RescheduleMeetup.Internal;

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
        var meetup = await context.Meetups
            .Include(meetup => meetup.Organizer)
            .SingleOrDefaultAsync(meetup => meetup.Id == request.MeetupId);
        if (meetup is null)
        {
            return Failure(ErrorTypes.MeetupDoesNotExist);
        }
        if (meetup.Organizer.Id != request.CurrentUserId)
        {
            return Failure(ErrorTypes.AccessViolation);
        }

        var city = await context.Cities.SingleOrDefaultAsync(city => city.Id == request.Place.CityId);
        if (city is null)
        {
            return Failure(ErrorTypes.CityDoesNotExist);
        }

        meetup.Reschedule(
            place: new MeetupPlace(city, request.Place.Address),
            duration: new MeetupDuration(request.Duration.Hours, request.Duration.Minutes),
            startTime: request.StartTime);
        await context.SaveChangesAsync();

        return Success(Result.NoPayload);
    }
}