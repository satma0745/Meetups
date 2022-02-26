namespace Meetups.Application.Features.Feed.GetMeetups.Api;

using System;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using Meetups.Application.Features.Shared.Infrastructure.Api;

public class RequestDto
{
    public PaginationDto Pagination { get; } = new();

    public FiltersDto Filters { get; } = new();
}

/// <remarks>
/// In this case, all DTO properties have setters. This was done on purpose,
/// because ASP cannot validate non-primitive DTO properties
/// if the DTO is passed via url query parameters
/// and nested DTO property values can only be set in the constructor.
/// </remarks>
public class PaginationDto
{
    private const int MaxAllowedPageSize = 50;

    /// <summary>Page number starting from 1.</summary>
    /// <example>1</example>
    [PublicAPI]
    [Range(1, int.MaxValue)]
    public int PageNumber { get; set; }

    /// <summary>Number of meetups per page.</summary>
    /// <example>20</example>
    [PublicAPI]
    [Range(1, MaxAllowedPageSize)]
    public int PageSize { get; set; }
    
    /// <summary>Determines meetups order before paging is applied.</summary>
    /// <example>topic_alphabetically</example>
    /// <seealso cref="OrderingOptions"/>
    [PublicAPI]
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

/// <remarks>
/// In this case, all DTO properties have setters. This was done on purpose,
/// because ASP cannot validate non-primitive DTO properties
/// if the DTO is passed via url query parameters
/// and nested DTO property values can only be set in the constructor.
/// </remarks>
public class FiltersDto
{
    /// <summary>Used to filter ot meetups that take place in other cities.</summary>
    /// <example>07450745-0745-0745-0745-074507450745</example>
    [PublicAPI]
    public Guid? CityId { get; set; }

    /// <summary>Used to find matching meetups.</summary>
    /// <example>Microsoft</example>
    [PublicAPI]
    [MaxLength(100)]
    public string Search { get; set; } = string.Empty;
}