namespace Meetups.Backend.Features.Auth.AuthenticateUser;

using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Meetups.Backend.Persistence.Entities;

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
}

public class ResponseDto
{
    /// <summary>Short-living token used for user authorization.</summary>
    public string AccessToken { get; }

    /// <summary>Long-living persisted token used to obtain new access tokens.</summary>
    public string RefreshToken { get; }

    public ResponseDto(string accessToken, string refreshToken)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
}

internal class MappingProfile : Profile
{
    public MappingProfile() =>
        CreateMap<RequestDto, User>()
            .ForMember(user => user.Password, config => config.Ignore());
}