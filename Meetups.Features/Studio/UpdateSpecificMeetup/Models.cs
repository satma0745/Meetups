namespace Meetups.Features.Studio.UpdateSpecificMeetup;

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using AutoMapper;
using Meetups.Features.Shared;
using Meetups.Persistence.Entities;

public class RequestDto
{
    /// <summary>Topic to be discussed on the meetup.</summary>
    /// <example>Microsoft naming issues</example>
    [Required]
    [MaxLength(100)]
    public string Topic { get; set; }
    
    /// <summary>Where meetup takes place.</summary>
    /// <example>Oslo</example>
    [Required]
    [MaxLength(75)]
    public string Place { get; set; }
    
    /// <summary>Meetup duration.</summary>
    [Required]
    [JsonConverter(typeof(MeetupDurationJsonConverter))]
    public Meetup.MeetupDuration Duration { get; set; }
    
    /// <summary>When meetup starts.</summary>
    /// <example>2022-01-09T12:00:00Z</example>
    [Required]
    public DateTime StartTime { get; set; }
}

internal class MappingProfile : Profile
{
    public MappingProfile() =>
        CreateMap<RequestDto, Meetup>();
}