namespace Meetups.Application.Features.Studio.RegisterNewMeetup.Api;

using Meetups.Application.Features.Shared.Contracts;
using Meetups.Application.Features.Shared.Infrastructure.Api;
using Meetups.Application.Features.Studio.RegisterNewMeetup.Internal;

internal static class Mappings
{
    public static Request ToInternalRequest(this RequestDto requestDto, CurrentUserInfo currentUser) =>
        new(
            topic: requestDto.Topic,
            cityId: requestDto.Place.CityId,
            address: requestDto.Place.Address,
            duration: requestDto.Duration.ToMeetupDuration(),
            startTime: requestDto.StartTime,
            currentUserId: currentUser.UserId);
}