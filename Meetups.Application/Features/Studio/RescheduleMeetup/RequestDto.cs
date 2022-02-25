namespace Meetups.Application.Features.Studio.RescheduleMeetup;

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Meetups.Application.Features.Shared.Contracts.PrimitiveDtos;

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