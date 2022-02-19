namespace Meetups.Backend.WebApi.Mapping;

using System.Reflection;
using Meetup.Contract;
using Meetups.Backend.Application;
using Microsoft.Extensions.DependencyInjection;

internal static class MappingConfiguration
{
    public static IServiceCollection AddMappings(this IServiceCollection services) =>
        services
            .AddAutoMapper<IContractMarker>()
            .AddAutoMapper<IApplicationMarker>()
            .AddAutoMapper(Assembly.GetExecutingAssembly());

    private static IServiceCollection AddAutoMapper<TMarker>(this IServiceCollection services) =>
        services.AddAutoMapper(typeof(TMarker));
}