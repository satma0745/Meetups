namespace Meetups.Backend.Entities.User;

using System;
using System.Collections.Generic;
using Meetups.Backend.Entities.Meetup;

public class Organizer : User
{
    #region Validation

    private static void EnsureValidMeetup(Meetup meetup)
    {
        if (meetup is null)
        {
            throw new ArgumentException("Must not be null.", nameof(meetup));
        }
    }
    
    #endregion
    
    #region State

    // Populated by the EF Core automatically when .Include is called
    public IReadOnlyCollection<Meetup> OrganizedMeetups => organizedMeetups;
    private readonly List<Meetup> organizedMeetups;

    #endregion

    #region Constructors

    public Organizer(string username, string password, string displayName)
        : base(username, password, displayName)
    {
        organizedMeetups = new List<Meetup>();
    }
    
    #endregion
    
    #region Behavior

    public void AddOrganizedMeetup(Meetup meetup)
    {
        EnsureValidMeetup(meetup);
        
        organizedMeetups.Add(meetup);
    }

    #endregion
}