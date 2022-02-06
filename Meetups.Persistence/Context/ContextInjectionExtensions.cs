namespace Meetups.Persistence.Context;

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class ContextInjectionExtensions
{
    public static IServiceCollection AddApplicationContext(this IServiceCollection services) =>
        services.AddDbContext<ApplicationContext>((options, configuration) =>
        {
            var connectionString = configuration.GetConnectionString("PostgreSQL");
            options.UseNpgsql(connectionString);
        });

    private static IServiceCollection AddDbContext<TContext>(
        this IServiceCollection services,
        Action<DbContextOptionsBuilder, IConfiguration> optionsAction)
        where TContext : DbContext =>
        services.AddDbContext<TContext>((serviceProvider, options) =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            optionsAction(options, configuration);
        });
}