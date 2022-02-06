namespace Meetups.WebApi.Auth;

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

internal static class AuthConfigurationExtensions
{
    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.ConfigureTokenValidationParameters(configuration);
                options.ClearClaimTypeMaps();
            });

        return services;
    }

    private static void ConfigureTokenValidationParameters(this JwtBearerOptions options, IConfiguration configuration)
    {
        var rawSigningKey = configuration["Auth:SecretKey"];
        var signingKeyBytes = Encoding.ASCII.GetBytes(rawSigningKey);
        
        options.TokenValidationParameters = new()
        {
            RequireSignedTokens = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes),

            ValidateAudience = false,
            ValidateIssuer = false,

            RequireExpirationTime = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
        
        options.RequireHttpsMetadata = false;
    }

    private static void ClearClaimTypeMaps(this JwtBearerOptions options)
    {
        var securityTokenValidators = options.SecurityTokenValidators;
        var securityTokenHandler = securityTokenValidators.OfType<JwtSecurityTokenHandler>().Single();
        
        securityTokenHandler.InboundClaimTypeMap.Clear();
        securityTokenHandler.OutboundClaimTypeMap.Clear();
    }
    
    public static IApplicationBuilder UseAuth(this IApplicationBuilder application) =>
        application
            .UseAuthentication()
            .UseAuthorization();
}