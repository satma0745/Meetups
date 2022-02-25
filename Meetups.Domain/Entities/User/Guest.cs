namespace Meetups.Domain.Entities.User;

using System;
using System.Collections.Generic;
using Meetups.Domain.Entities.Meetup;
using Meetups.Domain.Seedwork;

public class Guest : User
{
    #region Validation

    private static void EnsureValidMeetup(Meetup meetup) =>
        Assertions.EnsureValidObject(nameof(meetup), meetup, required: true);
    
    #endregion
    
    #region State

    // Populated by the EF Core automatically when .Include is called
    public IReadOnlyCollection<Meetup> MeetupsSignedUpTo => meetupsSignedUpTo;
    private readonly List<Meetup> meetupsSignedUpTo;

    #endregion
    
    #region Contructors

    public Guest(string username, string password, string displayName)
        : base(username, password, displayName)
    {
        meetupsSignedUpTo = new List<Meetup>();
    }
    
    #endregion
    
    #region Behavior

    public bool IsSignedUpFor(Meetup meetup) =>
        meetupsSignedUpTo.Contains(meetup);
    
    public void SignUpFor(Meetup meetup)
    {
        EnsureValidMeetup(meetup);
        
        if (IsSignedUpFor(meetup))
        {
            throw new ArgumentException("Already signed up for the specified meetup.", nameof(meetup));
        }
        
        meetupsSignedUpTo.Add(meetup);
    }

    public void CancelSignUpFor(Meetup meetup)
    {
        if (!IsSignedUpFor(meetup))
        {
            throw new ArgumentException("Guest should sign up for the meetup first.", nameof(meetup));
        }

        meetupsSignedUpTo.Remove(meetup);
    }
    
    #endregion
}