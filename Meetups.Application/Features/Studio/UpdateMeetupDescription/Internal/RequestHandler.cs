namespace Meetups.Application.Features.Studio.UpdateMeetupDescription.Internal;

using System.Threading.Tasks;
using Meetups.Application.Modules.Persistence;
using Meetups.Application.Seedwork.Internal;
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

        meetup.UpdateDescription(request.Topic);
        await context.SaveChangesAsync();

        return Success(Result.NoPayload);
    }
}