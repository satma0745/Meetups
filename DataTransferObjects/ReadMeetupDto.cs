namespace Meetups.DataTransferObjects;

using System;

public class ReadMeetupDto
{
    public Guid Id { get; set; }
    public string Topic { get; set; }
    public string Place { get; set; }
    public TimeSpan Duration { get; set; }
    public DateTime StartTime { get; set; }
}
