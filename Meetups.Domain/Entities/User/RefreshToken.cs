namespace Meetups.Domain.Entities.User;

using System;
using Meetups.Domain.Seedwork;

public class RefreshToken
{
    #region Validation

    private static void EnsureValidTokenId(Guid tokenId) =>
        Assertions.EnsureValidGuid(nameof(tokenId), tokenId, required: true);

    private static void EnsureValidBearerId(Guid bearerId) =>
        Assertions.EnsureValidGuid(nameof(bearerId), bearerId, required: true);

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