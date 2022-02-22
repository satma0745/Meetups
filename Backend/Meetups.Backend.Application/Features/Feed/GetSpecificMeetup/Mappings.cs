namespace Meetups.Backend.Application.Features.Feed.GetSpecificMeetup;

using Meetup.Contract.Models.Features.Feed.GetSpecificMeetup;
using Meetups.Backend.Application.Seedwork;
using Meetups.Backend.Entities.Meetup;

internal static class Mappings
{
    public static ResponseDto ToResponseDto(this Meetup meetup) =>
        new()
        {
            Id = meetup.Id,
            Topic = meetup.Topic,
            Place = meetup.Place.ToCustomMeetupPlaceDto(),
            Duration = meetup.Duration.ToMeetupDurationDto(),
            StartTime = meetup.StartTime,
            SignedUpGuestsCount = meetup.SignedUpGuests.Count
        };

    private static CustomMeetupPlaceDto ToCustomMeetupPlaceDto(this MeetupPlace meetupPlace) =>
        new()
        {
            CityId = meetupPlace.City.Id,
            CityName = meetupPlace.City.Name,
            Address = meetupPlace.Address
        };
}