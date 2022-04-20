namespace Meetups.Application.Features.Feed.GetMeetups.Internal;

using System.Linq;
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
        var meetups = await context.Meetups
            .Include(meetup => meetup.SignedUpGuests)
            .Include(meetup => meetup.Place.City)
            .ApplyFilters(request.Filters)
            .OrderBy(request.Pagination.OrderBy)
            .Paginate(request.Pagination)
            .ToListAsync();

        var result = new Result(
            meetups.Select(meetup => new MeetupModel(
                Id: meetup.Id,
                Topic: meetup.Topic,
                Place: new MeetupPlaceModel(meetup.Place.City.Id, meetup.Place.City.Name, meetup.Place.Address),
                StartTime: meetup.StartTime,
                Duration: new MeetupDurationModel(meetup.Duration.Hours, meetup.Duration.Minutes),
                SignedUpGuestsCount: meetup.SignedUpGuests.Count)));
        return Success(result);
    }
}