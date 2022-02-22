namespace Meetup.Contract.Models.Features.Studio.RegisterNewCity;

using System.ComponentModel.DataAnnotations;

public class RequestDto
{
    /// <summary>City name.</summary>
    /// <example>Oslo</example>
    [Required]
    [MaxLength(30)]
    public string Name { get; set; }
}