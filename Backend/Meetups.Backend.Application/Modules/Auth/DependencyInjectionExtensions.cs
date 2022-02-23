namespace Meetups.Backend.Application.Modules.Auth;

using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddAuthModule(this IServiceCollection services, AuthConfiguration configuration) =>
        services
            .AddScoped<ITokenHelper, TokenHelper>()
            .AddScoped(_ => configuration);
}