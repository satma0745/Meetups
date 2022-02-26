namespace Meetups.Application.Features.Feed.SignUpForMeetup.Internal;

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
            .SingleOrDefaultAsync(meetup => meetup.Id == request.MeetupId);
        if (meetup is null)
        {
            return Failure(ErrorTypes.MeetupDoesNotExist);
        }
        
        var currentUser = await context.Guests
            .Include(guest => guest.MeetupsSignedUpTo)
            .SingleAsync(guest => guest.Id == request.CurrentUserId);
        if (currentUser.IsSignedUpFor(meetup))
        {
            return Failure(ErrorTypes.AlreadySignedUp);
        }
        
        currentUser.SignUpFor(meetup);
        await context.SaveChangesAsync();

        return Success(Result.NoPayload);
    }
}