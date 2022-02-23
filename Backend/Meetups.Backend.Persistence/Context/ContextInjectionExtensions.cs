namespace Meetups.Backend.Persistence.Context;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class ContextInjectionExtensions
{
    public static IServiceCollection AddApplicationContext(this IServiceCollection services) =>
        services.AddDbContext<ApplicationContext>((serviceProvider, options) =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var persistenceConfiguration = PersistenceConfiguration.FromApplicationConfiguration(configuration);
            options.UseNpgsql(persistenceConfiguration.ConnectionString);
        });
}