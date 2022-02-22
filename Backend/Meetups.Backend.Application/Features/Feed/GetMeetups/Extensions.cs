namespace Meetups.Backend.Application.Features.Feed.GetMeetups;

using System.Linq;
using System.Runtime.CompilerServices;
using Meetup.Contract.Models.Features.Feed.GetMeetups;
using Meetups.Backend.Entities.Meetup;
using Microsoft.EntityFrameworkCore;

internal static class Extensions
{
    public static IQueryable<Meetup> ApplySearch(this IQueryable<Meetup> meetups, string pattern) =>
        meetups.Where(meetup => EF.Functions.Like(meetup.Topic, pattern) ||
                                EF.Functions.Like(meetup.Place.City.Name, pattern));

    public static IQueryable<Meetup> OrderBy(this IQueryable<Meetup> meetups, string orderingOption) =>
        orderingOption switch
        {
            OrderingOptions.TopicAlphabetically =>
                meetups.OrderBy(meetup => meetup.Topic),
            OrderingOptions.TopicReverseAlphabetically =>
                meetups.OrderByDescending(meetup => meetup.Topic),
            OrderingOptions.DurationAscending =>
                meetups.OrderBy(meetup => meetup.Duration.TotalMinutes),
            OrderingOptions.DurationDescending =>
                meetups.OrderByDescending(meetup => meetup.Duration.TotalMinutes),
            OrderingOptions.SignUpsCountAscending =>
                meetups.OrderBy(meetup => meetup.SignedUpGuests.Count),
            OrderingOptions.SignUpsCountDescending =>
                meetups.OrderByDescending(meetup => meetup.SignedUpGuests.Count),
            var unmatched =>
                throw new SwitchExpressionException(unmatched)
        };

    public static IQueryable<Meetup> Paginate(this IQueryable<Meetup> meetups, int pageNumber, int pageSize) =>
        meetups
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
}