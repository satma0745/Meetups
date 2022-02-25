namespace Meetups.Application.Modules.Auth.Injection;

using Meetups.Application.Modules.Auth.Configuration;
using Meetups.Application.Modules.Auth.TokenHelper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddAuthModule(
        this IServiceCollection services,
        IConfiguration applicationConfiguration,
        out IAuthConfiguration authConfiguration)
    {
        var configuration = AuthConfiguration.FromApplicationConfiguration(applicationConfiguration);
        authConfiguration = configuration;
        
        return services
            .AddScoped<ITokenHelper, TokenHelper>()
            .AddScoped<IAuthConfiguration>(_ => configuration);
    }
}