namespace Meetups.DataTransferObjects;

using System;

public class ReadMeetupDto
{
    /// <summary>Unique meetup identifier.</summary>
    public Guid Id { get; set; }
    
    /// <summary>Topic to be discussed on the meetup.</summary>
    public string Topic { get; set; }
    
    /// <summary>Where meetup takes place.</summary>
    public string Place { get; set; }
    
    /// <summary>Meetup duration.</summary>
    public TimeSpan Duration { get; set; }
    
    /// <summary>When meetup starts.</summary>
    public DateTime StartTime { get; set; }
}
