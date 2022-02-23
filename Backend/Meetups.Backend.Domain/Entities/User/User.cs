namespace Meetups.Backend.Domain.Entities.User;

using System;
using System.Collections.Generic;
using Meetups.Backend.Domain.Seedwork;

public abstract class User
{
    #region Validation

    private static void EnsureValidId(Guid id) =>
        Assertions.EnsureValidGuid(nameof(id), id, required: true);

    private static void EnsureValidUsername(string username) =>
        Assertions.EnsureValidString(nameof(username), username, required: true, minLength: 6, maxLength: 30);

    private static void EnsureValidPassword(string password) =>
        Assertions.EnsureValidString(nameof(password), password, required: true, exactLength: 60);

    private static void EnsureValidDisplayName(string displayName) =>
        Assertions.EnsureValidString(nameof(displayName), displayName, required: true, maxLength: 45);

    private static void EnsureValidRefreshToken(RefreshToken refreshToken) =>
        Assertions.EnsureValidObject(nameof(refreshToken), refreshToken, required: true);

    #endregion
    
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
        EnsureValidId(id);
        EnsureValidUsername(username);
        EnsureValidPassword(password);
        EnsureValidDisplayName(displayName);
        
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
        EnsureValidUsername(newUsername);
        EnsureValidPassword(newPassword);
        
        Username = newUsername;
        Password = newPassword;
    }

    public void AddRefreshToken(RefreshToken refreshToken)
    {
        EnsureValidRefreshToken(refreshToken);
        
        refreshTokens.Add(refreshToken);
    }

    public void ReplaceRefreshToken(RefreshToken oldRefreshToken, RefreshToken newRefreshToken)
    {
        // We don't actually need to validate `oldRefreshToken` since if it's in the list then it's already valid
        EnsureValidRefreshToken(newRefreshToken);
        
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