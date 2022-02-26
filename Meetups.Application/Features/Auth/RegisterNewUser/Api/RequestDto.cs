namespace Meetups.Application.Features.Auth.RegisterNewUser.Api;

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Meetups.Application.Features.Shared.Contracts.Auth;
using Meetups.Application.Features.Shared.Infrastructure.Api;

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
    
    /// <summary>Human readable public (official) name.</summary>
    /// <example>Satttttttttttttttter</example>
    [Required]
    [MaxLength(45)]
    public string DisplayName { get; }
    
    /// <summary>User account type (user role).</summary>
    /// <example>Guest</example>
    /// <seealso cref="UserRoles"/>
    [Required]
    [OneOf(typeof(UserRoles))]
    public string AccountType { get; }

    [JsonConstructor]
    public RequestDto(string username, string password, string displayName, string accountType)
    {
        Username = username;
        Password = password;
        DisplayName = displayName;
        AccountType = accountType;
    }
}