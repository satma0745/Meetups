namespace Meetups.Application.Features.Feed.GetMeetups.Api;

using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Meetups.Application.Features.Feed.GetMeetups.Internal;
using Meetups.Application.Features.Shared.Contracts;

internal static class Mappings
{
    public static Request ToInternalRequest(this RequestDto requestDto) =>
        new Request(
            pagination: new PaginationModel(
                pageNumber: requestDto.Pagination.PageNumber,
                pageSize: requestDto.Pagination.PageSize,
                orderBy: requestDto.Pagination.OrderBy.ToOrderingOption()),
            filters: new FiltersModel(
                cityId: requestDto.Filters.CityId,
                search: requestDto.Filters.Search));

    private static OrderingOption ToOrderingOption(this string stringOrderingOption) =>
        stringOrderingOption switch
        {
            OrderingOptions.TopicAlphabetically => OrderingOption.TopicAlphabetically,
            OrderingOptions.TopicReverseAlphabetically => OrderingOption.TopicReverseAlphabetically,
            OrderingOptions.DurationAscending => OrderingOption.DurationAscending,
            OrderingOptions.DurationDescending => OrderingOption.DurationDescending,
            OrderingOptions.SignUpsCountAscending => OrderingOption.SignUpsCountAscending,
            OrderingOptions.SignUpsCountDescending => OrderingOption.SignUpsCountDescending,
            var unmatched => throw new SwitchExpressionException(unmatched)
        };
    
    public static IEnumerable<ResponseDto> ToApiResponse(this Result internalResult) =>
        internalResult.Select(meetup => new ResponseDto(
            id: meetup.Id,
            topic: meetup.Topic,
            place: meetup.Place.ToCustomMeetupPlaceDto(),
            duration: meetup.Duration.ToMeetupDurationDto(),
            startTime: meetup.StartTime,
            signedUpGuestsCount: meetup.SignedUpGuestsCount));

    private static CustomMeetupPlaceDto ToCustomMeetupPlaceDto(this MeetupPlaceModel meetupPlace) =>
        new(meetupPlace.CityId, meetupPlace.CityName, meetupPlace.Address);
}