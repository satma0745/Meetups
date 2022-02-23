namespace Meetups.Backend.Application.Modules.Auth;

using System;
using System.Text;
using Meetup.Contract.Models.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

public class AuthConfiguration
{
    #region IConfiguration support
    
    public static AuthConfiguration FromApplicationConfiguration(IConfiguration configuration)
    {
        const string secretKeyPath = "Auth:SecretKey";
        var secretKey = configuration.GetValue<string>(secretKeyPath);
        if (string.IsNullOrWhiteSpace(secretKey))
        {
            throw ValidationException(secretKeyPath, "parameter is required");
        }

        const string accessTokenLifetimeInMinutesPath = "Auth:AccessTokenLifetimeInMinutes";
        var accessTokenLifetimeInMinutes = configuration.GetValue<int?>(accessTokenLifetimeInMinutesPath);
        if (accessTokenLifetimeInMinutes is null)
        {
            throw ValidationException(accessTokenLifetimeInMinutesPath, "parameter is required");
        }
        if (accessTokenLifetimeInMinutes <= 0)
        {
            throw ValidationException(accessTokenLifetimeInMinutesPath, "must be a positive number");
        }

        const string refreshTokenLifetimeInDaysPath = "Auth:RefreshTokenLifetimeInDays";
        var refreshTokenLifetimeInDays = configuration.GetValue<int?>(refreshTokenLifetimeInDaysPath);
        if (refreshTokenLifetimeInDays is null)
        {
            throw ValidationException(refreshTokenLifetimeInDaysPath, "parameter is required");
        }
        if (refreshTokenLifetimeInDays <= 0)
        {
            throw ValidationException(refreshTokenLifetimeInDaysPath, "must be a positive number");
        }

        return new AuthConfiguration(secretKey, accessTokenLifetimeInMinutes.Value, refreshTokenLifetimeInDays.Value);
    }

    private static Exception ValidationException(string path, string message) =>
        throw new($"Invalid value provided for the \"{path}\" configuration parameter: {message}.");
    
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