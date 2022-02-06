namespace Meetups.Features.Feed.GetMeetups;

using System.Linq;
using Meetups.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

internal static class Extensions
{
    public static IQueryable<Meetup> ApplySearch(this IQueryable<Meetup> meetups, string pattern) =>
        meetups.Where(meetup => EF.Functions.Like(meetup.Topic, pattern) ||
                                EF.Functions.Like(meetup.Place, pattern));

    public static IQueryable<Meetup> OrderBy(this IQueryable<Meetup> meetups, string orderingOption) =>
        orderingOption switch
        {
            "topic_asc" => meetups.OrderBy(meetup => meetup.Topic),
            "topic_desc" => meetups.OrderByDescending(meetup => meetup.Topic),
            "stime_asc" => meetups.OrderBy(meetup => meetup.StartTime),
            "stime_desc" => meetups.OrderByDescending(meetup => meetup.StartTime),
            _ => meetups
        };

    public static IQueryable<Meetup> Paginate(this IQueryable<Meetup> meetups, int pageNumber, int pageSize) =>
        meetups
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
}