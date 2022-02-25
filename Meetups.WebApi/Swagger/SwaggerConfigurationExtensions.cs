namespace Meetups.WebApi.Swagger;

using System;
using System.IO;
using Meetups.Application;
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
            options.SchemaFilter<OpenApiMeetupDurationDtoSchemaFilter>();

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
        var applicationProject = typeof(IApplicationMarker).Assembly;
        var xmlCommentsFileName = applicationProject.GetName().Name + ".xml";
        var pathToXmlCommentsFile = Path.Combine(AppContext.BaseDirectory, xmlCommentsFileName);
        
        options.IncludeXmlComments(pathToXmlCommentsFile);
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