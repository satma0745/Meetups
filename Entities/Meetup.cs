namespace Meetups.Entities;

using System;

public class Meetup
{
    public Guid Id { get; set; }
    
    public string Topic { get; set; }
    
    public string Place { get; set; }
    
    /// <summary> Duration in minutes. </summary>
    public int Duration { get; set; }
    
    public DateTime StartTime { get; set; }
}
