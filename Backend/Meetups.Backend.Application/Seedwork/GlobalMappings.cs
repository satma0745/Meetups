namespace Meetups.Backend.Application.Seedwork;

using Meetup.Contract.Models.Primitives;
using Meetups.Backend.Entities.Meetup;

internal static class GlobalMappings
{
    public static MeetupDuration ToMeetupDuration(this MeetupDurationDto meetupDurationDto) =>
        new(meetupDurationDto.Hours, meetupDurationDto.Minutes);

    public static MeetupDurationDto ToMeetupDurationDto(this MeetupDuration meetupDuration) =>
        new()
        {
            Hours = meetupDuration.Hours,
            Minutes = meetupDuration.Minutes
        };
}