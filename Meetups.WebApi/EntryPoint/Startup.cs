namespace Meetups.WebApi.EntryPoint;

using Meetups.Application.Features.Shared.Infrastructure.Internal;
using Meetups.Application.Modules.Persistence.Injection;
using Meetups.WebApi.Auth;
using Meetups.WebApi.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

internal class Startup
{
    private readonly IConfiguration configuration;

    public Startup(IConfiguration configuration) =>
        this.configuration = configuration;

    public void ConfigureServices(IServiceCollection services) =>
        services
            .AddInternalRequestHandlers()
            .AddPersistenceModule()
            .AddAuth(configuration)
            .AddSwaggerDocs()
            .AddControllers();

    public static void Configure(IApplicationBuilder application, IWebHostEnvironment environment) =>
        application
            .UseSwaggerDocs(environment)
            .UseRouting()
            .UseAuth()
            .UseEndpoints(endpoints => endpoints.MapControllers());
}