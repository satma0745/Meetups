namespace Meetups.Features.Feed.GetSignedUpGuestsInfo;

using System;
using AutoMapper;
using Meetups.Persistence.Entities;

public class SignedUpGuestInfo
{
    /// <summary>Permanent unique user identifier.</summary>
    /// <example>07450745-0745-0745-0745-074507450745</example>
    public Guid Id { get; set; }
    
    /// <summary>Human readable public (official) name.</summary>
    /// <example>Satttttttttttttttter</example>
    public string DisplayName { get; set; }
}

internal class MappingProfile : Profile
{
    public MappingProfile() =>
        CreateMap<Guest, SignedUpGuestInfo>();
}