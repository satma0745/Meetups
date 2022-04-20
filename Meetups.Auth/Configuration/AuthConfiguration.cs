namespace Meetups.Auth.Configuration;

using System;
using System.Text;
using Meetups.Application.Features.Shared.Auth;
using Meetups.Application.Modules.Auth;
using Meetups.Application.Seedwork.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

internal class AuthConfiguration : IAuthConfiguration
{
    #region IConfiguration support
    
    public static AuthConfiguration FromApplicationConfiguration(IConfiguration configuration)
    {
        const string secretKeyPath = "Auth:SecretKey"; 
        var secretKey = configuration
            .GetValue<string>(secretKeyPath)
            .Required(secretKeyPath);

        const string accessTokenLifetimeInMinutesPath = "Auth:AccessTokenLifetimeInMinutes";
        var accessTokenLifetimeInMinutes = configuration
            .GetValue<int?>(accessTokenLifetimeInMinutesPath)
            .Required(accessTokenLifetimeInMinutesPath)
            .EnsurePositive(accessTokenLifetimeInMinutesPath);

        const string refreshTokenLifetimeInDaysPath = "Auth:RefreshTokenLifetimeInDays";
        var refreshTokenLifetimeInDays = configuration
            .GetValue<int?>(refreshTokenLifetimeInDaysPath)
            .Required(refreshTokenLifetimeInDaysPath)
            .EnsurePositive(refreshTokenLifetimeInDaysPath);

        return new AuthConfiguration(secretKey, accessTokenLifetimeInMinutes, refreshTokenLifetimeInDays);
    }
    
    #endregion
    
    #region Parameters
    
    public SigningCredentials SigningCredentials { get; }
    
    public TimeSpan AccessTokenLifetime { get; }
    
    public TimeSpan RefreshTokenLifetime { get; }
    
    public TokenValidationParameters TokenValidationParameters { get; }
    
    #endregion
    
    #region Constructors

    private AuthConfiguration(string secretKey, int accessTokenLifetimeInMinutes, int refreshTokenLifetimeInDays)
    {
        var signingKeyBytes = Encoding.ASCII.GetBytes(secretKey);
        var signingKey = new SymmetricSecurityKey(signingKeyBytes);
        SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha512);
        
        AccessTokenLifetime = TimeSpan.FromMinutes(accessTokenLifetimeInMinutes);
        RefreshTokenLifetime = TimeSpan.FromDays(refreshTokenLifetimeInDays);

        TokenValidationParameters = new()
        {
            // Clear claim type map specifically for the user role claim
            RoleClaimType = AccessTokenPayload.UserRoleClaim,
            
            RequireSignedTokens = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = SigningCredentials.Key,

            ValidateAudience = false,
            ValidateIssuer = false,

            RequireExpirationTime = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    }
    
    #endregion
}
