namespace Meetups.Contract.Models.Features.Studio.RescheduleMeetup;

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Meetups.Contract.Models.Primitives;

public class RequestDto
{
    /// <summary>Where meetup will take place.</summary>
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
    public RequestDto(MeetupPlaceDto place, MeetupDurationDto duration, DateTime startTime)
    {
        Place = place;
        Duration = duration;
        StartTime = startTime;
    }
}