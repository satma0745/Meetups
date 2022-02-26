namespace Meetups.Application.Features.Feed.CancelMeetupSignup.Internal;

using System;

public record Request(
    Guid MeetupId,
    Guid CurrentUserId);