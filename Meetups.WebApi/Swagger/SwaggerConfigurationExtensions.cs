namespace Meetups.WebApi.Swagger;

using System;
using System.IO;
using Meetups.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

internal static class SwaggerConfigurationExtensions
{
    private const string ApiVersion = "v1";

    public static IServiceCollection AddSwaggerDocs(this IServiceCollection services) =>
        services
            .AddSwaggerGen(options =>
            {
                options.AddOpenApiInfo();
                options.IncludeXmlComments();
                options.CustomSchemaIds(modelType => modelType.FullName);
                options.AddJwtAuthSupport();
                options.SchemaFilter<OpenApiOneOfFilter>();
                options.SchemaFilter<OpenApiMeetupDurationDtoSchemaFilter>();
            })
            .AddSingleton(serviceProvider =>
            {
                var applicationConfiguration = serviceProvider.GetRequiredService<IConfiguration>();
                return SwaggerConfiguration.FromApplicationConfiguration(applicationConfiguration);
            });

    private static void AddOpenApiInfo(this SwaggerGenOptions options) =>
        options.SwaggerDoc(ApiVersion, new()
        {
            Version = ApiVersion,
            Title = "Meetups API",
            Description = "Meetups ASP .Net Core Web API"
        });

    private static void AddJwtAuthSupport(this SwaggerGenOptions options)
    {
        options.AddSecurityDefinition("Bearer", new()
        {
            Description = "Put Your access token here:",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT"
        });
        options.OperationFilter<OpenApiAuthFilter>();
    }

    private static void IncludeXmlComments(this SwaggerGenOptions options)
    {
        var applicationProject = typeof(IApplicationMarker).Assembly;
        var xmlCommentsFileName = applicationProject.GetName().Name + ".xml";
        var pathToXmlCommentsFile = Path.Combine(AppContext.BaseDirectory, xmlCommentsFileName);
        
        options.IncludeXmlComments(pathToXmlCommentsFile);
    }

    public static IApplicationBuilder UseSwaggerDocs(this IApplicationBuilder application)
    {
        var swaggerConfiguration = application.ApplicationServices.GetRequiredService<SwaggerConfiguration>();

        if (swaggerConfiguration.SwaggerEnabled)
        {
            application.UseSwagger();
            application.UseSwaggerUI(options =>
            {
                // Hide "Schemas" section
                if (!swaggerConfiguration.ShowSchemasInSwaggerUi)
                {
                    options.DefaultModelsExpandDepth(-1);
                }

                options.SwaggerEndpoint($"/swagger/{ApiVersion}/swagger.json", ApiVersion);
                options.RoutePrefix = swaggerConfiguration.SwaggerUiRoutePrefix;
            });
        }

        return application;
    }
}