namespace Meetups.Features.Auth.RegisterNewUser;

using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Meetups.Persistence.Entities;
using Meetups.WebApi.Validation;

public class RequestDto
{
    /// <summary>Public keyword used for authentication.</summary>
    /// <remarks>Also may be used as unique identifier.</remarks>
    /// <example>satma0745</example>
    [Required]
    [MinLength(6)]
    [MaxLength(30)]
    public string Username { get; set; }
    
    /// <summary>Private keyword used for authentication.</summary>
    /// <example>none_of_your_business</example>
    [Required]
    [MinLength(6)]
    [MaxLength(30)]
    public string Password { get; set; }
    
    /// <summary>Human readable public (official) name.</summary>
    /// <example>Satttttttttttttttter</example>
    [Required]
    [MaxLength(45)]
    public string DisplayName { get; set; }
    
    /// <summary>User account type (user role).</summary>
    /// <example>guest</example>
    [Required]
    [OneOf("guest", "organizer")]
    public string AccountType { get; set; }
}

internal class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RequestDto, Guest>().ForMember(user => user.Password, config => config.Ignore());
        CreateMap<RequestDto, Organizer>().ForMember(user => user.Password, config => config.Ignore());
    }
}