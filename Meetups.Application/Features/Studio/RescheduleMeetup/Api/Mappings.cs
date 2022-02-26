namespace Meetups.Application.Features.Studio.RescheduleMeetup.Api;

using System;
using Meetups.Application.Features.Shared.Infrastructure.Api;
using Meetups.Application.Features.Studio.RescheduleMeetup.Internal;

internal static class Mappings
{
    public static Request ToInternalRequest(this RequestDto requestDto, Guid meetupId, CurrentUserInfo currentUser) =>
        new Request(
            MeetupId: meetupId,
            Place: new MeetupPlaceModel(requestDto.Place.CityId, requestDto.Place.Address),
            StartTime: requestDto.StartTime,
            Duration: new MeetupDurationModel(requestDto.Duration.Hours, requestDto.Duration.Minutes),
            CurrentUserId: currentUser.UserId);
}