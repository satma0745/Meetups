namespace Meetups.Features.Meetup.GetMeetups;

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using AutoMapper;
using Meetups.Persistence.Entities;
using Meetups.WebApi.Json;
using Meetups.WebApi.Validation;

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

    /// <summary>Used to find matching meetups.</summary>
    /// <example>Microsoft</example>
    [MaxLength(100)]
    public string Search { get; set; } = string.Empty;
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
    [Required]
    [JsonConverter(typeof(MeetupDurationJsonConverter))]
    public Meetup.MeetupDuration Duration { get; set; }
    
    /// <summary>When meetup starts.</summary>
    /// <example>2022-01-09T12:00:00Z</example>
    public DateTime StartTime { get; set; }
    
    /// <summary>Number of users that signed up for this meetup.</summary>
    /// <example>42</example>
    public int SignedUpUsersCount { get; set; }
}

internal class MappingProfile : Profile
{
    public MappingProfile() =>
        CreateMap<Meetup, ResponseDto>()
            .ForMember(
                response => response.SignedUpUsersCount,
                options => options.MapFrom(meetup => meetup.SignedUpUsers.Count));
}