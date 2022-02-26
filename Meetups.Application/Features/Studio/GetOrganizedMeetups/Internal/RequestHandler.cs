namespace Meetups.Application.Features.Studio.GetOrganizedMeetups.Internal;

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
        var organizer = await context.Organizers
            .AsNoTracking()
            .Include(organizer => organizer.OrganizedMeetups)
            .ThenInclude(meetup => meetup.SignedUpGuests)
            .Include(organizer => organizer.OrganizedMeetups)
            .ThenInclude(meetup => meetup.Place.City)
            .SingleAsync(organizer => organizer.Id == request.CurrentUserId);

        var meetups = organizer.OrganizedMeetups.Select(meetup => new MeetupModel(
            id: meetup.Id,
            topic: meetup.Topic,
            place: new MeetupPlaceModel(meetup.Place.City.Id, meetup.Place.City.Name, meetup.Place.Address),
            duration: meetup.Duration,
            startTime: meetup.StartTime,
            signedUpGuestsCount: meetup.SignedUpGuests.Count));
        var result = new Result(meetups);
        return Success(result);
    }
}