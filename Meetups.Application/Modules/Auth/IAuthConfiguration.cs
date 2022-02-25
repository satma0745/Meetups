namespace Meetups.Application.Modules.Auth;

using System;
using Microsoft.IdentityModel.Tokens;

public interface IAuthConfiguration
{
    SigningCredentials SigningCredentials { get; }
    
    TimeSpan AccessTokenLifetime { get; }
    
    TimeSpan RefreshTokenLifetime { get; }
    
    TokenValidationParameters TokenValidationParameters { get; }
}