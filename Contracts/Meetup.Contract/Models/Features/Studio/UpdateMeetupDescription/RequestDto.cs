namespace Meetup.Contract.Models.Features.Studio.UpdateMeetupDescription;

using System.ComponentModel.DataAnnotations;

public class RequestDto
{
    /// <summary>Topic to be discussed on the meetup.</summary>
    /// <example>Microsoft naming issues</example>
    [Required]
    [MaxLength(100)]
    public string Topic { get; set; }
}