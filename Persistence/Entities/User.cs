namespace Meetups.Persistence.Entities;

using System;
using System.Collections.Generic;

public abstract class User
{
    public Guid Id { get; set; }
    
    public string Username { get; set; }
    
    public string Password { get; set; }
    
    public string DisplayName { get; set; }
}

public class Guest : User
{
    public ICollection<Meetup> MeetupsSignedUpTo { get; set; }
}

public class Organizer : User
{
    public ICollection<Meetup> OrganizedMeetups { get; set; }
}
