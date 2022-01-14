namespace Meetups.Persistence.Entities;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

[Index(nameof(Username), IsUnique = true)]
public class User
{
    public Guid Id { get; set; }
    
    [Required]
    [MaxLength(30)]
    public string Username { get; set; }
    
    [Required]
    [MaxLength(60)]
    public string Password { get; set; }
    
    [Required]
    [MaxLength(45)]
    public string DisplayName { get; set; }
    
    public ICollection<Meetup> MeetupsSignedUpTo { get; set; }
}
