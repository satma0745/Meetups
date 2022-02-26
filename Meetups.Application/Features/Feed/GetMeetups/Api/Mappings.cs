namespace Meetups.Application.Features.Feed.GetMeetups.Api;

using System.Linq;
using System.Runtime.CompilerServices;
using Meetups.Application.Features.Feed.GetMeetups.Internal;
using Meetups.Application.Features.Shared.Contracts.PrimitiveDtos;

internal static class Mappings
{
    public static Request ToInternalRequest(this RequestDto requestDto) =>
        new Request(
            Pagination: new PaginationModel(
                PageNumber: requestDto.Pagination.PageNumber,
                PageSize: requestDto.Pagination.PageSize,
                OrderBy: requestDto.Pagination.OrderBy.ToOrderingOption()),
            Filters: new FiltersModel(
                CityId: requestDto.Filters.CityId,
                Search: requestDto.Filters.Search));

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

    public static ResponseDto ToApiResponse(this Result internalResult)
    {
        var meetupDtos = internalResult.Select(ToMeetupDto);
        return new ResponseDto(meetupDtos);
    }

    private static MeetupDto ToMeetupDto(MeetupModel meetupModel) =>
        new MeetupDto(
            id: meetupModel.Id,
            topic: meetupModel.Topic,
            place: new MeetupPlaceDto(meetupModel.Place.CityId, meetupModel.Place.CityName, meetupModel.Place.Address),
            startTime: meetupModel.StartTime,
            duration: new MeetupDurationDto(meetupModel.Duration.Hours, meetupModel.Duration.Minutes),
            signedUpGuestsCount: meetupModel.SignedUpGuestsCount);
}