namespace Meetups.Application.Seedwork.Internal;

using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

public static class RequestHandlersInjectionExtensions
{
    public static IServiceCollection AddInternalRequestHandlers(this IServiceCollection services)
    {
        var requestHandlerTypes = Assembly
            .GetExecutingAssembly()
            .DefinedTypes
            .Where(type => type.IsClass && !type.IsAbstract)
            .Where(type => type.IsSubclassOfGeneric(typeof(RequestHandlerBase<,,>)));

        foreach (var requestHandlerType in requestHandlerTypes)
        {
            services.AddScoped(requestHandlerType);
        }
        return services;
    }
    
    private static bool IsSubclassOfGeneric(this Type @class, Type candidate)
    {
        while (@class is not null)
        {
            var current = @class.IsGenericType
                ? @class.GetGenericTypeDefinition()
                : @class;

            if (candidate == current)
            {
                return true;
            }

            @class = @class.BaseType;
        }

        return false;
    }
}