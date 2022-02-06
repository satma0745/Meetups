namespace Meetups.WebApi.Swagger;

using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

/// <summary>Marks some actions with a padlock icon in the Swagger UI</summary>
internal class OpenApiAuthFilter : IOperationFilter
{
    private static readonly OpenApiSecurityRequirement AuthenticationRequirement = new()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            Array.Empty<string>()
        }
    };

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var actionMetadata = context.ApiDescription.ActionDescriptor.EndpointMetadata;
        
        // Require authentication if action is marked with the [Authorize] attribute
        var markedWithAuthorize = actionMetadata.Any(metadataItem => metadataItem is AuthorizeAttribute);
        if (markedWithAuthorize)
        {
            operation.Security.Add(AuthenticationRequirement);
        }
    }
}