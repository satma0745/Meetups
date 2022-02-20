namespace Meetups.Backend.Entities.User;

using System;

public class RefreshToken
{
    #region State
    
    public Guid TokenId { get; }
    
    public Guid BearerId { get; }
    
    #endregion

    #region Constructors

    public RefreshToken(Guid tokenId, Guid bearerId)
    {
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