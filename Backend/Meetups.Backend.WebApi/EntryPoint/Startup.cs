namespace Meetups.Backend.WebApi.EntryPoint;

using Meetups.Backend.Application.Modules.Persistence.Injection;
using Meetups.Backend.WebApi.Auth;
using Meetups.Backend.WebApi.Swagger;
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