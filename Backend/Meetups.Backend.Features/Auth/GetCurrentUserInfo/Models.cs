namespace Meetups.Backend.Features.Auth.GetCurrentUserInfo;

using System;
using AutoMapper;
using Meetups.Backend.Persistence.Entities;

public class ResponseDto
{
    /// <summary>Permanent unique user identifier.</summary>
    /// <example>07450745-0745-0745-0745-074507450745</example>
    public Guid Id { get; set; }
    
    /// <summary>Public keyword used for authentication.</summary>
    /// <remarks>Also may be used as unique identifier.</remarks>
    /// <example>satma0745</example>
    public string Username { get; set; }
    
    /// <summary>Human readable public (official) name.</summary>
    /// <example>Satttttttttttttttter</example>
    public string DisplayName { get; set; }
}

internal class MappingProfile : Profile
{
    public MappingProfile() =>
        CreateMap<User, ResponseDto>();
}