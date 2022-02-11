namespace Meetup.Contract.Models.Features.Feed.GetSignedUpGuestsInfo;

using System;

public class ResponseDto
{
    /// <summary>Permanent unique user identifier.</summary>
    /// <example>07450745-0745-0745-0745-074507450745</example>
    public Guid Id { get; set; }
    
    /// <summary>Human readable public (official) name.</summary>
    /// <example>Satttttttttttttttter</example>
    public string DisplayName { get; set; }
}