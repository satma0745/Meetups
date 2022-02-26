namespace Meetups.Application.Features.Feed.GetSpecificMeetup.Internal;

using System;
using Meetups.Domain.Entities.Meetup;

public class Result
{
    public Guid Id { get; }
    
    public string Topic { get; }
    
    public MeetupPlaceModel Place { get; }
    
    public MeetupDuration Duration { get; }
    
    public DateTime StartTime { get; }
    
    public int SignedUpGuestsCount { get; }

    public Result(
        Guid id,
        string topic,
        MeetupPlaceModel place,
        MeetupDuration duration,
        DateTime startTime,
        int signedUpGuestsCount)
    {
        Id = id;
        Topic = topic;
        Place = place;
        Duration = duration;
        StartTime = startTime;
        SignedUpGuestsCount = signedUpGuestsCount;
    }
}

public class MeetupPlaceModel
{
    public Guid CityId { get; }
    
    public string CityName { get; }
    
    public string Address { get; }

    public MeetupPlaceModel(Guid cityId, string cityName, string address)
    {
        CityId = cityId;
        CityName = cityName;
        Address = address;
    }
}