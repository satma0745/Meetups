namespace Meetups.Application.Features.Feed.GetSpecificMeetup.Internal;

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
        var meetup = await context.Meetups
            .AsNoTracking()
            .Include(meetup => meetup.SignedUpGuests)
            .Include(meetup => meetup.Place.City)
            .SingleOrDefaultAsync(meetup => meetup.Id == request.MeetupId);
        if (meetup is null)
        {
            return Failure(ErrorTypes.MeetupDoesNotExist);
        }

        var result = new Result(
            id: meetup.Id,
            topic: meetup.Topic,
            place: new MeetupPlaceModel(meetup.Place.City.Id, meetup.Place.City.Name, meetup.Place.Address),
            duration: meetup.Duration,
            startTime: meetup.StartTime,
            signedUpGuestsCount: meetup.SignedUpGuests.Count);
        return Success(result);
    }
}