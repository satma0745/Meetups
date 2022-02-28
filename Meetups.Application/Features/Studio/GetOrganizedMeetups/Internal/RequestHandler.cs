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
            .Include(organizer => organizer.OrganizedMeetups)
            .ThenInclude(meetup => meetup.SignedUpGuests)
            .Include(organizer => organizer.OrganizedMeetups)
            .ThenInclude(meetup => meetup.Place.City)
            .SingleAsync(organizer => organizer.Id == request.OrganizerId);

        var meetups = organizer.OrganizedMeetups.Select(meetup => new MeetupModel(
            Id: meetup.Id,
            Topic: meetup.Topic,
            Place: new MeetupPlaceModel(meetup.Place.City.Id, meetup.Place.City.Name, meetup.Place.Address),
            StartTime: meetup.StartTime,
            Duration: new MeetupDurationModel(meetup.Duration.Hours, meetup.Duration.Minutes),
            SignedUpGuestsCount: meetup.SignedUpGuests.Count));
        var result = new Result(meetups);
        return Success(result);
    }
}