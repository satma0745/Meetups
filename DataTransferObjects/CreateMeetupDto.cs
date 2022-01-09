namespace Meetups.DataTransferObjects;

using System;

public class CreateMeetupDto
{
    public string Topic { get; set; }
    
    public string Place { get; set; }
    
    /// <summary> Duration in minutes. </summary>
    public int Duration { get; set; }
    
    public DateTime StartTime { get; set; }
}
