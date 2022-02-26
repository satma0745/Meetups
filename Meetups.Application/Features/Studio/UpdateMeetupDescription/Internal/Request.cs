namespace Meetups.Application.Features.Studio.UpdateMeetupDescription.Internal;

using System;

public record Request(
    Guid MeetupId,
    string Topic,
    Guid CurrentUserId);