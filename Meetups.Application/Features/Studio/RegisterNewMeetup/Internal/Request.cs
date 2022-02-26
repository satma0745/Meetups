namespace Meetups.Application.Features.Studio.RegisterNewMeetup.Internal;

using System;
using Meetups.Domain.Entities.Meetup;

public class Request
{
    public string Topic { get; }
    
    public Guid CityId { get; }
    
    public string Address { get; }
    
    public MeetupDuration Duration { get; }
    
    public DateTime StartTime { get; }
    
    public Guid CurrentUserId { get; }

    public Request(
        string topic,
        Guid cityId,
        string address,
        MeetupDuration duration,
        DateTime startTime,
        Guid currentUserId)
    {
        Topic = topic;
        CityId = cityId;
        Address = address;
        Duration = duration;
        StartTime = startTime;
        CurrentUserId = currentUserId;
    }
}