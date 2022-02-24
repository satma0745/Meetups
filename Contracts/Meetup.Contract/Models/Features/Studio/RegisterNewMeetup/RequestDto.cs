namespace Meetup.Contract.Models.Features.Studio.RegisterNewMeetup;

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Meetup.Contract.Models.Primitives;

public class RequestDto
{
    /// <summary>Topic to be discussed on the meetup.</summary>
    /// <example>Microsoft naming issues</example>
    [Required]
    [MaxLength(100)]
    public string Topic { get; }
    
    /// <inheritdoc cref="MeetupPlaceDto"/>
    [Required]
    public MeetupPlaceDto Place { get; }
    
    /// <summary>Meetup duration.</summary>
    [Required]
    public MeetupDurationDto Duration { get; }
    
    /// <summary>When meetup starts.</summary>
    /// <example>2022-01-09T12:00:00Z</example>
    [Required]
    public DateTime StartTime { get; }

    [JsonConstructor]
    public RequestDto(string topic, MeetupPlaceDto place, MeetupDurationDto duration, DateTime startTime)
    {
        Topic = topic;
        Place = place;
        Duration = duration;
        StartTime = startTime;
    }
}