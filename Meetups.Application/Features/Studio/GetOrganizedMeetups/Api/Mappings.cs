namespace Meetups.Application.Features.Studio.GetOrganizedMeetups.Api;

using System.Linq;
using Meetups.Application.Features.Shared.Contracts.PrimitiveDtos;
using Meetups.Application.Features.Studio.GetOrganizedMeetups.Internal;

internal static class Mappings
{
    public static ResponseDto ToApiResponse(this Result internalResult)
    {
        var meetupDtos = internalResult.Select(ToMeetupDto);
        return new ResponseDto(meetupDtos);
    }

    private static MeetupDto ToMeetupDto(MeetupModel meetup) =>
        new MeetupDto(
            id: meetup.Id,
            topic: meetup.Topic,
            place: new MeetupPlaceDto(meetup.Place.CityId, meetup.Place.CityName, meetup.Place.Address),
            startTime: meetup.StartTime,
            duration: new MeetupDurationDto(meetup.Duration.Hours, meetup.Duration.Minutes),
            signedUpGuestsCount: meetup.SignedUpGuestsCount
        );
}