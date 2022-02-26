namespace Meetups.Application.Features.Feed.GetSpecificMeetup.Api;

using Meetups.Application.Features.Feed.GetSpecificMeetup.Internal;
using Meetups.Application.Features.Shared.Contracts;

internal static class Mappings
{
    public static ResponseDto ToApiResponse(this Result internalResult) =>
        new ResponseDto(
            id: internalResult.Id,
            topic: internalResult.Topic,
            place: internalResult.Place.ToCustomMeetupPlaceDto(),
            duration: internalResult.Duration.ToMeetupDurationDto(),
            startTime: internalResult.StartTime,
            signedUpGuestsCount: internalResult.SignedUpGuestsCount);

    private static CustomMeetupPlaceDto ToCustomMeetupPlaceDto(this MeetupPlaceModel meetupPlace) =>
        new(meetupPlace.CityId, meetupPlace.CityName, meetupPlace.Address);
}