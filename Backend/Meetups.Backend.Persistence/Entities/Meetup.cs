namespace Meetups.Backend.Persistence.Entities;

using System;
using System.Collections.Generic;

public class Meetup
{
    public Guid Id { get; set; }
    
    public string Topic { get; set; }
    
    public string Place { get; set; }
    
    public MeetupDuration Duration { get; set; }
    
    public DateTime StartTime { get; set; }
    
    public Organizer Organizer { get; set; }
    
    public ICollection<Guest> SignedUpGuests { get; set; }

    public class MeetupDuration
    {
        public int Hours { get; set; }
    
        public int Minutes { get; set; }
    }
}
