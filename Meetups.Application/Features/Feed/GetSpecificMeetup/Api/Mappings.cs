namespace Meetups.Application.Features.Feed.GetSpecificMeetup.Api;

using Meetups.Application.Features.Feed.GetSpecificMeetup.Internal;
using Meetups.Application.Features.Shared.Contracts.PrimitiveDtos;

internal static class Mappings
{
    public static ResponseDto ToApiResponse(this Result internalResult) =>
        new ResponseDto(
            id: internalResult.Id,
            topic: internalResult.Topic,
            place: new MeetupPlaceDto(
                cityId: internalResult.Place.CityId,
                cityName: internalResult.Place.CityName,
                address: internalResult.Place.Address),
            startTime: internalResult.StartTime,
            duration: new MeetupDurationDto(
                hours: internalResult.Duration.Hours,
                minutes: internalResult.Duration.Minutes),
            signedUpGuestsCount: internalResult.SignedUpGuestsCount);
}