namespace Meetups.Features.Feed.GetMeetups;

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
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
    /// <example>topic_alphabetically</example>
    [Required]
    [OneOf(typeof(OrderingOptions))]
    public string OrderBy { get; set; }

    /// <summary>Used to find matching meetups.</summary>
    /// <example>Microsoft</example>
    [MaxLength(100)]
    public string Search { get; set; } = string.Empty;
}

public static class OrderingOptions
{
    public const string TopicAlphabetically = "topic_alphabetically";
    public const string TopicReverseAlphabetically = "topic_reverse_alphabetically";

    public const string DurationAscending = "duration_ascending";
    public const string DurationDescending = "duration_descending";

    public const string SignUpsCountAscending = "signups_ascending";
    public const string SignUpsCountDescending = "signups_descending";
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
    
    /// <summary>Number of guests that signed up for this meetup.</summary>
    /// <example>42</example>
    public int SignedUpGuestsCount { get; set; }
}

internal class MappingProfile : Profile
{
    public MappingProfile() =>
        CreateMap<Meetup, ResponseDto>()
            .ForMember(
                response => response.SignedUpGuestsCount,
                options => options.MapFrom(meetup => meetup.SignedUpGuests.Count));
}