namespace Meetups.Application.Features.Feed.SignUpForMeetup.Internal;

using System;

public record Request(
    Guid MeetupId,
    Guid CurrentUserId);