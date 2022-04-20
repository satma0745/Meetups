namespace Meetups.Auth.Injection;

using Meetups.Application.Modules.Auth;
using Meetups.Auth.Configuration;
using Meetups.Auth.TokenHelper;
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
