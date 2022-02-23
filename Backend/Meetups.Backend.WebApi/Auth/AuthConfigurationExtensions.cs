namespace Meetups.Backend.WebApi.Auth;

using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Meetups.Backend.Application.Modules.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

internal static class AuthConfigurationExtensions
{
    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var authConfiguration = AuthConfiguration.FromApplicationConfiguration(configuration);
        
        services.AddAuthModule(authConfiguration);
        
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.ConfigureTokenValidationParameters(authConfiguration);
                options.ClearClaimTypeMaps();
            });

        return services;
    }

    private static void ConfigureTokenValidationParameters(
        this JwtBearerOptions options,
        AuthConfiguration configuration)
    {
        options.TokenValidationParameters = configuration.TokenValidationParameters;
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