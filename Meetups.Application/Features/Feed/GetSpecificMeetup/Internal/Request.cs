namespace Meetups.Application.Features.Feed.GetSpecificMeetup.Internal;

using System;

public class Request
{
    public Guid MeetupId { get; }

    public Request(Guid meetupId) =>
        MeetupId = meetupId;
}