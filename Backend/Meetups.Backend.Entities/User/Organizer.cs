namespace Meetups.Backend.Entities.User;

using System.Collections.Generic;
using Meetups.Backend.Entities.Meetup;

public class Organizer : User
{
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

    public void AddOrganizedMeetup(Meetup meetup) =>
        organizedMeetups.Add(meetup);

    #endregion
}