namespace Meetups.Persistence.Entities;

using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

[Index(nameof(Topic), IsUnique = true)]
public class Meetup
{
    public Guid Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Topic { get; set; }
    
    [Required]
    [MaxLength(75)]
    public string Place { get; set; }
    
    public TimeSpan Duration { get; set; }
    
    public DateTime StartTime { get; set; }
}
