namespace Meetups.Persistence.Entities;

using System;
using System.Collections.Generic;

public class User
{
    public Guid Id { get; set; }
    
    public string Username { get; set; }
    
    public string Password { get; set; }
    
    public string DisplayName { get; set; }
    
    public ICollection<Meetup> MeetupsSignedUpTo { get; set; }
}
