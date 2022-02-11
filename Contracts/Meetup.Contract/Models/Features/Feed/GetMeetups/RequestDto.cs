namespace Meetup.Contract.Models.Features.Feed.GetMeetups;

using System.ComponentModel.DataAnnotations;
using Meetup.Contract.Attributes;

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
    /// <seealso cref="OrderingOptions"/>
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