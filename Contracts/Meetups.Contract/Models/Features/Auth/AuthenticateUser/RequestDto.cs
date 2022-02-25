namespace Meetups.Contract.Models.Features.Auth.AuthenticateUser;

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class RequestDto
{
    /// <summary>Public keyword used for authentication.</summary>
    /// <remarks>Also may be used as unique identifier.</remarks>
    /// <example>satma0745</example>
    [Required]
    [MinLength(6)]
    [MaxLength(30)]
    public string Username { get; }
    
    /// <summary>Private keyword used for authentication.</summary>
    /// <example>none_of_your_business</example>
    [Required]
    [MinLength(6)]
    [MaxLength(30)]
    public string Password { get; }

    [JsonConstructor]
    public RequestDto(string username, string password)
    {
        Username = username;
        Password = password;
    }
}