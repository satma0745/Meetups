namespace Meetups.Application.Features.Feed.GetSignedUpGuestsInfo.Internal;

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
        var meetup = await context.Meetups
            .Include(meetup => meetup.SignedUpGuests)
            .SingleOrDefaultAsync(meetup => meetup.Id == request.MeetupId);
        if (meetup is null)
        {
            return Failure(ErrorTypes.MeetupDoesNotExist);
        }

        var guests = meetup.SignedUpGuests.Select(guest => new GuestModel(guest.Id, guest.DisplayName));
        var result = new Result(guests);
        return Success(result);
    }
}