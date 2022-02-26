namespace Meetups.Application.Features.Feed.GetSignedUpGuestsInfo.Api;

using System;
using System.Collections.Generic;
using JetBrains.Annotations;

public class ResponseDto : List<GuestDto>
{
    public ResponseDto(IEnumerable<GuestDto> guests)
        : base(guests)
    {
    }
}

public class GuestDto
{
    /// <summary>Permanent unique user identifier.</summary>
    /// <example>07450745-0745-0745-0745-074507450745</example>
    [PublicAPI]
    public Guid Id { get; }
    
    /// <summary>Human readable public (official) name.</summary>
    /// <example>Satttttttttttttttter</example>
    [PublicAPI]
    public string DisplayName { get; }

    public GuestDto(Guid id, string displayName)
    {
        Id = id;
        DisplayName = displayName;
    }
}