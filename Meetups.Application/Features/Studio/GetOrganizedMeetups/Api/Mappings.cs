namespace Meetups.Application.Features.Studio.GetOrganizedMeetups.Api;

using System.Collections.Generic;
using System.Linq;
using Meetups.Application.Features.Shared.Contracts;
using Meetups.Application.Features.Studio.GetOrganizedMeetups.Internal;

internal static class Mappings
{
    public static IEnumerable<ResponseDto> ToApiResponse(this Result internalResult) =>
        internalResult.Select(meetup =>
            new ResponseDto(
                id: meetup.Id,
                topic: meetup.Topic,
                place: meetup.Place.ToCustomMeetupPlaceDto(),
                duration: meetup.Duration.ToMeetupDurationDto(),
                startTime: meetup.StartTime,
                signedUpGuestsCount: meetup.SignedUpGuestsCount
            )
        );

    private static CustomMeetupPlaceDto ToCustomMeetupPlaceDto(this MeetupPlaceModel meetupPlace) =>
        new(meetupPlace.CityId, meetupPlace.CityName, meetupPlace.Address);
}