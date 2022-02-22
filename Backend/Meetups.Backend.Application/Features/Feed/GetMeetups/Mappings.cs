namespace Meetups.Backend.Application.Features.Feed.GetMeetups;

using Meetup.Contract.Models.Features.Feed.GetMeetups;
using Meetups.Backend.Application.Seedwork;
using Meetups.Backend.Entities.Meetup;

internal static class Mappings
{
    public static ResponseDto ToResponseDto(this Meetup meetup) =>
        new()
        {
            Id = meetup.Id,
            Topic = meetup.Topic,
            Place = meetup.Place,
            Duration = meetup.Duration.ToMeetupDurationDto(),
            StartTime = meetup.StartTime,
            SignedUpGuestsCount = meetup.SignedUpGuests.Count
        };
}