namespace Meetups.Application.Features.Studio.DeleteSpecificMeetup.Internal;

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
        var meetupToDelete = await context.Meetups.SingleOrDefaultAsync(meetup => meetup.Id == request.MeetupId);
        if (meetupToDelete is null)
        {
            return Failure(ErrorTypes.MeetupDoesNotExist);
        }
        
        var organizer = await context.Organizers
            .Include(organizer => organizer.OrganizedMeetups)
            .SingleAsync(organizer => organizer.Id == request.CurrentUserId);
        if (organizer.OrganizedMeetups.All(organizedMeetup => organizedMeetup.Id != request.MeetupId))
        {
            return Failure(ErrorTypes.AccessViolation);
        }

        context.Meetups.Remove(meetupToDelete);
        await context.SaveChangesAsync();

        return Success(Result.NoPayload);
    }
}