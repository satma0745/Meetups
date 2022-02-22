namespace Meetup.Contract.Models.Features.Feed.GetMeetups;

using System;
using System.ComponentModel.DataAnnotations;
using Meetup.Contract.Attributes;

public class RequestDto
{
    public PaginationDto Pagination { get; set; }

    public FiltersDto Filters { get; } = FiltersDto.Empty;
}

public class PaginationDto
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
    /// <seealso cref="OrderingOptions"/>
    [Required]
    [OneOf(typeof(OrderingOptions))]
    public string OrderBy { get; set; }
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

public class FiltersDto
{
    public static FiltersDto Empty => new()
    {
        CityId = null,
        Search = string.Empty
    };
    
    /// <summary>Used to filter ot meetups that take place in other cities.</summary>
    /// <example>07450745-0745-0745-0745-074507450745</example>
    public Guid? CityId { get; set; }
    
    /// <summary>Used to find matching meetups.</summary>
    /// <example>Microsoft</example>
    [MaxLength(100)]
    public string Search { get; set; }
}