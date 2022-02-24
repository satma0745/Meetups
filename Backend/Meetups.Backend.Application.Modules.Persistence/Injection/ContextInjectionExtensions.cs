namespace Meetups.Backend.Application.Modules.Persistence.Injection;

using Meetups.Backend.Application.Modules.Persistence.Configuration;
using Meetups.Backend.Application.Modules.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class ContextInjectionExtensions
{
    public static IServiceCollection AddPersistenceModule(this IServiceCollection services) =>
        services.AddDbContext<IApplicationContext, ApplicationContext>((serviceProvider, options) =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var persistenceConfiguration = PersistenceConfiguration.FromApplicationConfiguration(configuration);
            options.UseNpgsql(persistenceConfiguration.ConnectionString);
        });
}