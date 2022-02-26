namespace Meetups.Application.Features.Feed.SignUpForMeetup.Internal;

using System;

public class Request
{
    public Guid MeetupId { get; }
    
    public Guid CurrentUserId { get; }

    public Request(Guid meetupId, Guid currentUserId)
    {
        MeetupId = meetupId;
        CurrentUserId = currentUserId;
    }
}