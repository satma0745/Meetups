namespace Meetups.Backend.Application.Seedwork;

using Meetup.Contract.Models.Primitives;
using Meetups.Backend.Entities.Meetup;

internal static class GlobalMappings
{
    #region MeetupDuration

    public static MeetupDuration ToMeetupDuration(this MeetupDurationDto meetupDurationDto) =>
        new(meetupDurationDto.Hours, meetupDurationDto.Minutes);

    public static MeetupDurationDto ToMeetupDurationDto(this MeetupDuration meetupDuration) =>
        new()
        {
            Hours = meetupDuration.Hours,
            Minutes = meetupDuration.Minutes
        };

    #endregion
    
    #region MeetupPlace

    public static MeetupPlace ToMeetupPlace(this MeetupPlaceDto meetupPlaceDto, City city) =>
        new(city, meetupPlaceDto.Address);

    public static MeetupPlaceDto ToMeetupPlaceDto(this MeetupPlace meetupPlace) =>
        new()
        {
            CityId = meetupPlace.City.Id,
            Address = meetupPlace.Address
        };

    #endregion
}