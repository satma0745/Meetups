namespace Meetups.Application.Features.Shared.Contracts.Auth;

using System;

public class RefreshTokenPayload
{
    public const string TokenIdClaim = "jti";
    public const string BearerIdClaim = "sub";
    
    /// <summary>Id of the refresh token itself.</summary>
    /// <example>07450745-0745-0745-0745-074507450745</example>
    public Guid TokenId { get; }
    
    /// <summary>Token bearer's id.</summary>
    /// <example>07450745-0745-0745-0745-074507450745</example>
    public Guid BearerId { get; }

    public RefreshTokenPayload(Guid tokenId, Guid bearerId)
    {
        TokenId = tokenId;
        BearerId = bearerId;
    }
}