namespace Meetups.Backend.Application.Features.Studio.GetOrganizedMeetups;

using Meetups.Backend.Application.Features.Seedwork;
using Meetups.Backend.Domain.Entities.Meetup;
using Meetups.Contract.Models.Features.Studio.GetOrganizedMeetups;

internal static class Mappings
{
    public static ResponseDto ToResponseDto(this Meetup meetup) =>
        new ResponseDto(
            id: meetup.Id,
            topic: meetup.Topic,
            place: meetup.Place.ToCustomMeetupPlaceDto(),
            duration: meetup.Duration.ToMeetupDurationDto(),
            startTime: meetup.StartTime,
            signedUpGuestsCount: meetup.SignedUpGuests.Count
        );

    private static CustomMeetupPlaceDto ToCustomMeetupPlaceDto(this MeetupPlace meetupPlace) =>
        new(meetupPlace.City.Id, meetupPlace.City.Name, meetupPlace.Address);
}