namespace Meetups.DataTransferObjects;

using System;
using System.ComponentModel.DataAnnotations;

public class CreateMeetupDto
{
    [Required]
    [MaxLength(100)]
    public string Topic { get; set; }
    
    [Required]
    [MaxLength(75)]
    public string Place { get; set; }
    
    /// <summary> Duration in minutes. </summary>
    [Range(120, 720)]
    public int Duration { get; set; }
    
    [Required]
    public DateTime StartTime { get; set; }
}
