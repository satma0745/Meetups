namespace Meetups.DataTransferObjects.User;

using System.ComponentModel.DataAnnotations;

public class AuthenticateUserDto
{
    /// <summary>Public keyword used for authentication.</summary>
    /// <remarks>Also may be used as unique identifier.</remarks>
    /// <example>satma0745</example>
    [Required]
    public string Username { get; set; }
    
    /// <summary>Private keyword used for authentication.</summary>
    /// <example>none_of_your_business</example>
    [Required]
    public string Password { get; set; }
}