namespace Meetups.Application.Features.Studio.RegisterNewCity;

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class RequestDto
{
    /// <summary>City name.</summary>
    /// <example>Oslo</example>
    [Required]
    [MaxLength(30)]
    public string Name { get; }

    [JsonConstructor]
    public RequestDto(string name) =>
        Name = name;
}