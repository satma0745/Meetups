namespace Meetups.Application.Features.Studio.RegisterNewMeetup.Api;

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Meetups.Application.Features.Shared.PrimitiveDtos;

public class RequestDto
{
    /// <summary>Topic to be discussed on the meetup.</summary>
    /// <example>Microsoft naming issues</example>
    [Required]
    [MaxLength(100)]
    public string Topic { get; }
    
    /// <summary>Where meetup will take place.</summary>
    [Required]
    public MeetupPlaceDto Place { get; }
    
    /// <summary>When meetup starts.</summary>
    /// <example>2022-01-09T12:00:00Z</example>
    [Required]
    public DateTime StartTime { get; }
    
    /// <summary>Meetup duration.</summary>
    [Required]
    public MeetupDurationDto Duration { get; }

    [JsonConstructor]
    public RequestDto(string topic, MeetupPlaceDto place, DateTime startTime, MeetupDurationDto duration)
    {
        Topic = topic;
        Place = place;
        StartTime = startTime;
        Duration = duration;
    }
}

public class MeetupPlaceDto
{
    /// <summary>Id of the city where the meetup will take place.</summary>
    /// <example>07450745-0745-0745-0745-074507450745</example>
    public Guid CityId { get; }
    
    /// <summary>The address where the meetup will take place.</summary>
    /// <example>Frank street, building 17, floor 7, office 41</example>
    [Required]
    [MaxLength(75)]
    public string Address { get; }

    [JsonConstructor]
    public MeetupPlaceDto(Guid cityId, string address)
    {
        CityId = cityId;
        Address = address;
    }
}