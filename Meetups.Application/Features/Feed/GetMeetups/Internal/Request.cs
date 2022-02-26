namespace Meetups.Application.Features.Feed.GetMeetups.Internal;

using System;

public record Request(
    PaginationModel Pagination,
    FiltersModel Filters);

public record PaginationModel(
    int PageNumber,
    int PageSize,
    OrderingOption OrderBy);

public enum OrderingOption
{
    TopicAlphabetically,
    TopicReverseAlphabetically,
    DurationAscending,
    DurationDescending,
    SignUpsCountAscending,
    SignUpsCountDescending,
}

public record FiltersModel(
    Guid? CityId,
    string Search);