namespace Meetups.Backend.Entities.User;

using System.Collections.Generic;
using Meetups.Backend.Entities.Meetup;

public class Organizer : User
{
    #region State

    public ICollection<Meetup> OrganizedMeetups { get; set; }    

    #endregion

    #region Constructors

    public Organizer(string username, string password, string displayName)
        : base(username, password, displayName)
    {
    }
    
    #endregion
}