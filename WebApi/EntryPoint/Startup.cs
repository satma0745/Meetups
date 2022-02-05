namespace Meetups.WebApi.EntryPoint;

using System.Reflection;
using Meetups.Features.Shared;
using Meetups.Persistence.Context;
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
            .AddApplicationContext()
            .AddScoped<TokenHelper>()
            .AddAutoMapper(Assembly.GetExecutingAssembly())
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