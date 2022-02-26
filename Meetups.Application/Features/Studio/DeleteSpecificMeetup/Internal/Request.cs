namespace Meetups.Application.Features.Studio.DeleteSpecificMeetup.Internal;

using System;

public record Request(
    Guid MeetupId,
    Guid CurrentUserId);