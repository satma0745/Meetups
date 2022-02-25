namespace Meetups.Domain.Entities.User;

using System.Collections.Generic;
using Meetups.Domain.Entities.Meetup;
using Meetups.Domain.Seedwork;

public class Organizer : User
{
    #region Validation

    private static void EnsureValidMeetup(Meetup meetup) =>
        Assertions.EnsureValidObject(nameof(meetup), meetup, required: true);
    
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