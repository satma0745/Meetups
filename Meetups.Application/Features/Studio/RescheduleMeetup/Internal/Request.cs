namespace Meetups.Application.Features.Studio.RescheduleMeetup.Internal;

using System;
using Meetups.Domain.Entities.Meetup;

public class Request
{
    public Guid MeetupId { get; }
    
    public Guid CityId { get; }
    
    public string Address { get; }
    
    public DateTime StartTime { get; }
    
    public MeetupDuration Duration { get; }
    
    public Guid CurrentUserId { get; }

    public Request(
        Guid meetupId,
        Guid cityId,
        string address,
        DateTime startTime,
        MeetupDuration duration,
        Guid currentUserId)
    {
        MeetupId = meetupId;
        CityId = cityId;
        Address = address;
        StartTime = startTime;
        Duration = duration;
        CurrentUserId = currentUserId;
    }
}