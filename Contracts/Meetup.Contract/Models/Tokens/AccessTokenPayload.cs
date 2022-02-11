namespace Meetup.Contract.Models.Tokens;

using System;

public class AccessTokenPayload
{
    public const string UserIdClaim = "sub";
    public const string UserRoleClaim = "role";
    
    /// <summary>Token bearer's id.</summary>
    /// <example>07450745-0745-0745-0745-074507450745</example>
    public Guid UserId { get; set; }
    
    /// <summary>Role of the token bearer.</summary>
    /// <example>Guest</example>
    /// <seealso cref="Meetup.Contract.Models.Enumerations.UserRoles"/>
    public string UserRole { get; set; }
}