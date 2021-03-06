namespace Meetups.Application.Features.Studio.UpdateMeetupDescription.Api;

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class RequestDto
{
    /// <summary>Topic to be discussed on the meetup.</summary>
    /// <example>Microsoft naming issues</example>
    [Required]
    [MaxLength(100)]
    public string Topic { get; }

    [JsonConstructor]
    public RequestDto(string topic) =>
        Topic = topic;
}