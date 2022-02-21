namespace Meetups.Backend.Entities.User;

using System;
using System.Collections.Generic;

public abstract class User
{
    #region State
    
    public Guid Id { get; }
    
    public string Username { get; private set; }
    
    public string Password { get; private set; }
    
    public string DisplayName { get; }
    
    // Populated by the EF Core automatically when .Include is called
    public IReadOnlyCollection<RefreshToken> RefreshTokens => refreshTokens;
    private readonly List<RefreshToken> refreshTokens;

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

        refreshTokens = new List<RefreshToken>();
    }
    
    #endregion

    #region Behavior

    public void ChangeCredentials(string newUsername, string newPassword)
    {
        Username = newUsername;
        Password = newPassword;
    }

    public void AddRefreshToken(RefreshToken refreshToken) =>
        refreshTokens.Add(refreshToken);

    public void ReplaceRefreshToken(RefreshToken oldRefreshToken, RefreshToken newRefreshToken)
    {
        if (!refreshTokens.Contains(oldRefreshToken))
        {
            throw new ArgumentException("Unable to replace non-existent token.", nameof(oldRefreshToken));
        }

        refreshTokens.Remove(oldRefreshToken);
        refreshTokens.Add(newRefreshToken);
    }

    public void RevokeAllRefreshTokens() =>
        refreshTokens.Clear();

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