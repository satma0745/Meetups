namespace Meetup.Contract.Models.Features.Studio.RegisterNewMeetup;

using System;
using System.ComponentModel.DataAnnotations;
using Meetup.Contract.Models.Primitives;

public class RequestDto
{
    /// <summary>Topic to be discussed on the meetup.</summary>
    /// <example>Microsoft naming issues</example>
    [Required]
    [MaxLength(100)]
    public string Topic { get; set; }
    
    /// <inheritdoc cref="MeetupPlaceDto"/>
    [Required]
    public MeetupPlaceDto Place { get; set; }
    
    /// <summary>Meetup duration.</summary>
    [Required]
    public MeetupDurationDto Duration { get; set; }
    
    /// <summary>When meetup starts.</summary>
    /// <example>2022-01-09T12:00:00Z</example>
    [Required]
    public DateTime StartTime { get; set; }
}