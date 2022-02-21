namespace Meetups.Backend.Entities.User;

using System;
using System.Collections.Generic;

public abstract class User
{
    #region Validation
    
    private static void EnsureValidId(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Must not be empty.", nameof(id));
        }
    }

    private static void EnsureValidUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException("Must not be null or empty.", nameof(username));
        }

        const int minLength = 6;
        const int maxLength = 30;
        if (username.Length < minLength)
        {
            throw new ArgumentException($"Must be at least {minLength} characters long.", nameof(username));
        }
        if (username.Length > maxLength)
        {
            throw new ArgumentException($"Must not exceed {maxLength} characters.", nameof(username));
        }
    }

    private static void EnsureValidPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Must not be null or empty.", nameof(password));
        }

        const int exactLength = 60;
        if (password.Length != exactLength)
        {
            throw new ArgumentException($"Must be exactly {exactLength} characters long.", nameof(password));
        }
    }

    private static void EnsureValidDisplayName(string displayName)
    {
        if (string.IsNullOrWhiteSpace(displayName))
        {
            throw new ArgumentException("Must not be null or empty.", nameof(displayName));
        }

        const int maxLength = 45;
        if (displayName.Length > maxLength)
        {
            throw new ArgumentException($"Must not exceed {maxLength} characters.", nameof(displayName));
        }
    }

    private static void EnsureValidRefreshToken(RefreshToken refreshToken)
    {
        if (refreshToken is null)
        {
            throw new ArgumentException("Must not be null.", nameof(refreshToken));
        }
    }

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