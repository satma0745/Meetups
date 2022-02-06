namespace Meetups.Persistence.Entities;

using System;
using System.Collections.Generic;

public abstract class User
{
    public Guid Id { get; set; }
    
    public string Username { get; set; }
    
    public string Password { get; set; }
    
    public string DisplayName { get; set; }
    
    /// <remarks>
    /// Populated <b>automatically</b> by EF Core. The <b>only</b> valid values for this field are represented as
    /// constant string fields inside <see cref="UserRoles"/> static class.
    /// </remarks>
    public string Role { get; set; }
}

public static class UserRoles
{
    public const string Guest = "Guest";
    public const string Organizer = "Organizer";
}

public class Guest : User
{
    public ICollection<Meetup> MeetupsSignedUpTo { get; set; }
}

public class Organizer : User
{
    public ICollection<Meetup> OrganizedMeetups { get; set; }
}
