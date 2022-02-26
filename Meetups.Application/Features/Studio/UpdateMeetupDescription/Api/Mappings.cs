namespace Meetups.Application.Features.Studio.UpdateMeetupDescription.Api;

using System;
using Meetups.Application.Features.Shared.Infrastructure.Api;
using Meetups.Application.Features.Studio.UpdateMeetupDescription.Internal;

internal static class Mappings
{
    public static Request ToInternalRequest(this RequestDto requestDto, Guid meetupId, CurrentUserInfo currentUser) =>
        new(meetupId, requestDto.Topic, currentUser.UserId);
}