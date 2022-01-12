namespace Meetups.Features.Meetup.RegisterNewMeetup;

using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
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
    
    /// <summary>Meetup duration (in minutes).</summary>
    /// <example>180 </example>
    [Range(120, 720)]
    public int Duration { get; set; }
    
    /// <summary>When meetup starts.</summary>
    /// <example>2022-01-09T12:00:00Z</example>
    [Required]
    public DateTime StartTime { get; set; }
}

internal class MappingProfile : Profile
{
    public MappingProfile() =>
        CreateMap<RequestDto, Meetup>()
            .ForMember(meetup => meetup.Duration, config => config.MapFrom(dto => TimeSpan.FromMinutes(dto.Duration)));
}