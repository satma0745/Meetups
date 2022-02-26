namespace Meetups.Application.Features.Studio.RegisterNewMeetup.Internal;

using System.Threading.Tasks;
using Meetups.Application.Features.Shared.Infrastructure.Internal;
using Meetups.Application.Modules.Persistence;
using Meetups.Domain.Entities.Meetup;
using Microsoft.EntityFrameworkCore;

public class RequestHandler : RequestHandlerBase<Request, Result, ErrorTypes>
{
    private readonly IApplicationContext context;

    public RequestHandler(IApplicationContext context) =>
        this.context = context;
    
    public override async Task<Response<Result, ErrorTypes>> HandleRequest(Request request)
    {
        var topicTaken = await context.Meetups.AnyAsync(meetup => meetup.Topic == request.Topic);
        if (topicTaken)
        {
            return Failure(ErrorTypes.TopicAlreadyTaken);
        }

        var city = await context.Cities.SingleOrDefaultAsync(city => city.Id == request.CityId);
        if (city is null)
        {
            return Failure(ErrorTypes.CityDoesNotExist);
        }

        var organizer = await context.Organizers
            .Include(organizer => organizer.OrganizedMeetups)
            .ThenInclude(meetup => meetup.Place.City)
            .SingleAsync(organizer => organizer.Id == request.CurrentUserId);

        var meetup = new Meetup(
            topic: request.Topic,
            place: new MeetupPlace(city, request.Address),
            duration: request.Duration,
            startTime: request.StartTime);
        organizer.AddOrganizedMeetup(meetup);
        
        await context.SaveChangesAsync();

        return Success(Result.NoPayload);
    }
}