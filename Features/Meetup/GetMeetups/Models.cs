namespace Meetups.Features.Meetup.GetMeetups;

using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Meetups.Features.Shared;
using Meetups.Persistence.Entities;

public class RequestDto
{
    private const int MaxAllowedPageSize = 50;

    /// <summary>Page number starting from 1.</summary>
    /// <example>1</example>
    [Range(1, int.MaxValue)]
    public int PageNumber { get; set; }

    /// <summary>Number of meetups per page.</summary>
    /// <example>20</example>
    [Range(1, MaxAllowedPageSize)]
    public int PageSize { get; set; }
    
    /// <summary>Determines meetups order before paging is applied.</summary>
    /// <remarks>Only "topic_asc", "topic_desc", "stime_asc" and "stime_desc" are considered valid values.</remarks>
    /// <example>topic_asc</example>
    [Required]
    [OneOf("topic_asc", "topic_desc", "stime_asc", "stime_desc")]
    public string OrderBy { get; set; }
}

public class ResponseDto
{
    /// <summary>Permanent unique meetup identifier.</summary>
    /// <example>07450745-0745-0745-0745-074507450745</example>
    public Guid Id { get; set; }
    
    /// <summary>Topic to be discussed on the meetup.</summary>
    /// <example>Microsoft naming issues</example>
    public string Topic { get; set; }
    
    /// <summary>Where meetup takes place.</summary>
    /// <example>Oslo</example>
    public string Place { get; set; }
    
    /// <summary>Meetup duration.</summary>
    /// <example>"03:00:00"</example>
    public TimeSpan Duration { get; set; }
    
    /// <summary>When meetup starts.</summary>
    /// <example>2022-01-09T12:00:00Z</example>
    public DateTime StartTime { get; set; }
}

internal class MappingProfile : Profile
{
    public MappingProfile() =>
        CreateMap<Meetup, ResponseDto>();
}