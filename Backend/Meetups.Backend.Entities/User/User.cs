namespace Meetups.Backend.Entities.User;

using System;

public abstract class User
{
    #region State
    
    public Guid Id { get; }
    
    public string Username { get; private set; }
    
    public string Password { get; private set; }
    
    public string DisplayName { get; private set; }
    
    #endregion
    
    #region Constructors

    public User(string username, string password, string displayName)
        : this(id: Guid.NewGuid(), username, password, displayName)
    {
    }

    private User(Guid id, string username, string password, string displayName)
    {
        Id = id;
        Username = username;
        Password = password;
        DisplayName = displayName;
    }
    
    #endregion

    #region Behavior

    public void ChangeCredentials(string newUsername, string newPassword)
    {
        Username = newUsername;
        Password = newPassword;
    }

    #endregion
    
    #region Equality

    public override int GetHashCode() =>
        Id.GetHashCode();

    public override bool Equals(object obj) =>
        ReferenceEquals(obj, this) ||
        obj is User other &&
        other.GetType() == GetType() &&
        other.Id == Id &&
        other.Username == Username &&
        other.Password == Password &&
        other.DisplayName == DisplayName;
    
    #endregion
}