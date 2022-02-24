namespace Meetups.Backend.Application.Features.Studio.GetOrganizedMeetups;

using Meetup.Contract.Models.Features.Studio.GetOrganizedMeetups;
using Meetups.Backend.Application.Features.Seedwork;
using Meetups.Backend.Domain.Entities.Meetup;

internal static class Mappings
{
    public static ResponseDto ToResponseDto(this Meetup meetup) =>
        new()
        {
            Id = meetup.Id,
            Topic = meetup.Topic,
            Place = meetup.Place.ToMeetupPlaceDto(),
            Duration = meetup.Duration.ToMeetupDurationDto(),
            StartTime = meetup.StartTime,
            SignedUpGuestsCount = meetup.SignedUpGuests.Count
        };
}