namespace Meetup.Contract.Models.Tokens;

using System;

public class RefreshTokenPayload
{
    public const string UserIdClaim = "sub";
    public const string TokenIdClaim = "jti";
    
    /// <summary>Token bearer's id.</summary>
    /// <example>07450745-0745-0745-0745-074507450745</example>
    public Guid UserId { get; set; }
    
    /// <summary>Id of the refresh token itself.</summary>
    /// <example>07450745-0745-0745-0745-074507450745</example>
    public Guid TokenId { get; set; }
}