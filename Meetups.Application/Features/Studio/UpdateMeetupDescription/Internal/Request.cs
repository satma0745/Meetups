namespace Meetups.Application.Features.Studio.UpdateMeetupDescription.Internal;

using System;

public class Request
{
    public Guid MeetupId { get; }
    
    public string Topic { get; }
    
    public Guid CurrentUserId { get; }

    public Request(Guid meetupId, string topic, Guid currentUserId)
    {
        MeetupId = meetupId;
        Topic = topic;
        CurrentUserId = currentUserId;
    }
}