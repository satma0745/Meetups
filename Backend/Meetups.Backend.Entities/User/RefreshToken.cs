namespace Meetups.Backend.Entities.User;

using System;

public class RefreshToken
{
    #region Validation

    private static void EnsureValidTokenId(Guid tokenId)
    {
        if (tokenId == Guid.Empty)
        {
            throw new ArgumentException("Must not be empty.", nameof(tokenId));
        }
    }

    private static void EnsureValidBearerId(Guid bearerId)
    {
        if (bearerId == Guid.Empty)
        {
            throw new ArgumentException("Must not be empty.", nameof(bearerId));
        }
    }

    #endregion
    
    #region State
    
    public Guid TokenId { get; }
    
    public Guid BearerId { get; }
    
    #endregion

    #region Constructors

    public RefreshToken(Guid tokenId, Guid bearerId)
    {
        EnsureValidTokenId(tokenId);
        EnsureValidBearerId(bearerId);
        
        TokenId = tokenId;
        BearerId = bearerId;
    }

    #endregion
    
    #region Equality

    public override int GetHashCode() =>
        HashCode.Combine(TokenId, BearerId);

    public override bool Equals(object obj) =>
        ReferenceEquals(obj, this) ||
        obj is RefreshToken other &&
        other.TokenId == TokenId &&
        other.BearerId == BearerId;
    
    #endregion
}