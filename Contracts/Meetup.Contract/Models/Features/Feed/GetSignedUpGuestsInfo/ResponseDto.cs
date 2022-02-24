namespace Meetup.Contract.Models.Features.Feed.GetSignedUpGuestsInfo;

using System;
using JetBrains.Annotations;

public class ResponseDto
{
    /// <summary>Permanent unique user identifier.</summary>
    /// <example>07450745-0745-0745-0745-074507450745</example>
    [PublicAPI]
    public Guid Id { get; }
    
    /// <summary>Human readable public (official) name.</summary>
    /// <example>Satttttttttttttttter</example>
    [PublicAPI]
    public string DisplayName { get; }

    public ResponseDto(Guid id, string displayName)
    {
        Id = id;
        DisplayName = displayName;
    }
}