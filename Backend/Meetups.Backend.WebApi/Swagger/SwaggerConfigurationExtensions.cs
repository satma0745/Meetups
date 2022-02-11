namespace Meetups.Backend.WebApi.Swagger;

using System;
using System.IO;
using System.Linq;
using Meetup.Contract;
using Meetups.Backend.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

internal static class SwaggerConfigurationExtensions
{
    private const string ApiVersion = "v1";

    public static IServiceCollection AddSwaggerDocs(this IServiceCollection services) =>
        services.AddSwaggerGen(options =>
        {
            options.AddOpenApiInfo();
            options.IncludeXmlComments();
            options.CustomSchemaIds(modelType => modelType.FullName);
            options.AddJwtAuthSupport();
            options.SchemaFilter<OpenApiOneOfFilter>();
            options.SchemaFilter<OpenApiMeetupDurationSchemaFilter>();

        });

    private static void AddOpenApiInfo(this SwaggerGenOptions options) =>
        options.SwaggerDoc(ApiVersion, new()
        {
            Version = ApiVersion,
            Title = "Meetups API",
            Description = "Meetup CRUD (hopefully not only CRUD) ASP .Net Core Web API",
            Contact = new()
            {
                Name = "Satma0745",
                Url = new Uri("https://t.me/satma0745")
            }
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
        var projectsWithXmlComments = new[]
        {
            typeof(IContractMarker).Assembly,
            typeof(ICoreMarker).Assembly
        };
        
        var xmlCommentsFiles = projectsWithXmlComments
            .Select(project => project.GetName().Name)
            .Select(projectName => $"{projectName}.xml");
        
        var rootDirectory = AppContext.BaseDirectory;
        foreach (var xmlCommentsFile in xmlCommentsFiles)
        {
            options.IncludeXmlComments(Path.Combine(rootDirectory, xmlCommentsFile));
        }
    }

    public static IApplicationBuilder UseSwaggerDocs(
        this IApplicationBuilder application,
        IWebHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            application.UseSwagger();
            application.UseSwaggerUI(options =>
            {
                // Hide "Schemas" section
                options.DefaultModelsExpandDepth(-1);

                // Serve Swagger UI on the "/api" url
                options.SwaggerEndpoint($"/swagger/{ApiVersion}/swagger.json", ApiVersion);
                options.RoutePrefix = "api";
            });
        }

        return application;
    }
}