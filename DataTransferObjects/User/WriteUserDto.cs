﻿namespace Meetups.DataTransferObjects.User;

using System.ComponentModel.DataAnnotations;

public class WriteUserDto
{
    /// <summary>Keyword used for authorization.</summary>
    /// <remarks>Also may be used as unique identifier.</remarks>
    /// <example>satma0745</example>
    [Required]
    [MinLength(6)]
    [MaxLength(30)]
    public string Username { get; set; }
    
    /// <summary>Keyword used for authentication.</summary>
    /// <example>none_of_your_business</example>
    [Required]
    [MinLength(6)]
    [MaxLength(30)]
    public string Password { get; set; }
    
    /// <summary>Human readable public (official) name.</summary>
    /// <example>Satttttttttttttttter</example>
    [Required]
    [MaxLength(45)]
    public string DisplayName { get; set; }
}