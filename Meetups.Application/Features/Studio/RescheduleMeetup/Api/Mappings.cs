namespace Meetups.Application.Features.Studio.RescheduleMeetup.Api;

using System;
using Meetups.Application.Features.Shared.Contracts;
using Meetups.Application.Features.Shared.Infrastructure.Api;
using Meetups.Application.Features.Studio.RescheduleMeetup.Internal;

internal static class Mappings
{
    public static Request ToInternalRequest(this RequestDto requestDto, Guid meetupId, CurrentUserInfo currentUser) =>
        new(meetupId: meetupId,
            cityId: requestDto.Place.CityId,
            address: requestDto.Place.Address,
            startTime: requestDto.StartTime,
            duration: requestDto.Duration.ToMeetupDuration(),
            currentUserId: currentUser.UserId);
}