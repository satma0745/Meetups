﻿namespace Meetups.Backend.WebApi.Swagger;

using Meetups.Contract.Models.Primitives;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

internal class OpenApiMeetupDurationDtoSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type != typeof(MeetupDurationDto))
        {
            return;
        }
        
        schema.Type = "string";
        schema.Pattern = @"^\d+:\d{2}$";
        schema.Example = new OpenApiString("3:00");
    }
}