namespace Meetups.Application.Features.Feed.GetMeetups.Internal;

using System.Linq;
using System.Runtime.CompilerServices;
using Meetups.Domain.Entities.Meetup;
using Microsoft.EntityFrameworkCore;

internal static class Extensions
{
    public static IQueryable<Meetup> ApplyFilters(this IQueryable<Meetup> meetups, FiltersModel filters) =>
        meetups
            .Where(meetup => filters.CityId == null || meetup.Place.City.Id == filters.CityId)
            .Where(meetup => EF.Functions.Like(meetup.Topic, $"%{filters.Search}%"));

    public static IQueryable<Meetup> OrderBy(this IQueryable<Meetup> meetups, OrderingOption orderingOption) =>
        orderingOption switch
        {
            OrderingOption.TopicAlphabetically =>
                meetups.OrderBy(meetup => meetup.Topic),
            OrderingOption.TopicReverseAlphabetically =>
                meetups.OrderByDescending(meetup => meetup.Topic),
            OrderingOption.DurationAscending =>
                meetups.OrderBy(meetup => meetup.Duration.TotalMinutes),
            OrderingOption.DurationDescending =>
                meetups.OrderByDescending(meetup => meetup.Duration.TotalMinutes),
            OrderingOption.SignUpsCountAscending =>
                meetups.OrderBy(meetup => meetup.SignedUpGuests.Count),
            OrderingOption.SignUpsCountDescending =>
                meetups.OrderByDescending(meetup => meetup.SignedUpGuests.Count),
            var unmatched =>
                throw new SwitchExpressionException(unmatched)
        };

    public static IQueryable<Meetup> Paginate(this IQueryable<Meetup> meetups, PaginationModel pagination) =>
        meetups
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize);
}