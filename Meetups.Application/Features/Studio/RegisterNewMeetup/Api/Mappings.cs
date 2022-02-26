namespace Meetups.Application.Features.Studio.RegisterNewMeetup.Api;

using Meetups.Application.Features.Shared.Infrastructure.Api;
using Meetups.Application.Features.Studio.RegisterNewMeetup.Internal;

internal static class Mappings
{
    public static Request ToInternalRequest(this RequestDto requestDto, CurrentUserInfo currentUser) =>
        new Request(
            Topic: requestDto.Topic,
            Place: new MeetupPlaceModel(requestDto.Place.CityId, requestDto.Place.Address),
            StartTime: requestDto.StartTime,
            Duration: new MeetupDurationModel(requestDto.Duration.Hours, requestDto.Duration.Minutes),
            CurrentUserId: currentUser.UserId);
}