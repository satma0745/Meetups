namespace Meetup.Contract.Models.Features.Auth.GetCurrentUserInfo;

using System;
using JetBrains.Annotations;

public class ResponseDto
{
    /// <summary>Permanent unique user identifier.</summary>
    /// <example>07450745-0745-0745-0745-074507450745</example>
    [PublicAPI]
    public Guid Id { get; }
    
    /// <summary>Public keyword used for authentication.</summary>
    /// <remarks>Also may be used as unique identifier.</remarks>
    /// <example>satma0745</example>
    [PublicAPI]
    public string Username { get; }
    
    /// <summary>Human readable public (official) name.</summary>
    /// <example>Satttttttttttttttter</example>
    [PublicAPI]
    public string DisplayName { get; }

    public ResponseDto(Guid id, string username, string displayName)
    {
        Id = id;
        Username = username;
        DisplayName = displayName;
    }
}