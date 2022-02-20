namespace Meetups.Backend.Entities.User;

using System.Collections.Generic;
using Meetups.Backend.Entities.Meetup;

public class Guest : User
{
    #region State
    
    public ICollection<Meetup> MeetupsSignedUpTo { get; set; }
    
    #endregion
    
    #region Contructors

    public Guest(string username, string password, string displayName)
        : base(username, password, displayName)
    {
    }
    
    #endregion
}