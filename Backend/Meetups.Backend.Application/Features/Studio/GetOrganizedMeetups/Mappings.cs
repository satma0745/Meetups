namespace Meetups.Backend.Application.Features.Studio.GetOrganizedMeetups;

using Meetup.Contract.Models.Features.Studio.GetOrganizedMeetups;
using Meetups.Backend.Application.Features.Seedwork;
using Meetups.Backend.Domain.Entities.Meetup;

internal static class Mappings
{
    public static ResponseDto ToResponseDto(this Meetup meetup) =>
        new ResponseDto(
            id: meetup.Id,
            topic: meetup.Topic,
            place: meetup.Place.ToMeetupPlaceDto(),
            duration: meetup.Duration.ToMeetupDurationDto(),
            startTime: meetup.StartTime,
            signedUpGuestsCount: meetup.SignedUpGuests.Count
        );
}