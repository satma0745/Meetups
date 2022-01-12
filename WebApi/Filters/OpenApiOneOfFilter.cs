namespace Meetups.WebApi.Filters;

using System;
using System.Linq;
using Meetups.Features.Shared;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

internal class OpenApiOneOfFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var oneOfAttribute = (OneOfAttribute) context.MemberInfo?
            .GetCustomAttributes(typeof(OneOfAttribute), inherit: false)
            .SingleOrDefault();
        if (oneOfAttribute is null)
        {
            return;
        }

        var allowedValues = oneOfAttribute.AllowedValues;
        schema.Enum = allowedValues.Select(ToOpenApiAny).ToList();
    }

    private static IOpenApiAny ToOpenApiAny(object value) =>
        value switch
        {
            null => new OpenApiNull(),
            string @string => new OpenApiString(@string),
            bool @bool => new OpenApiBoolean(@bool),
            int @int => new OpenApiInteger(@int),
            _ => throw new ArgumentOutOfRangeException(nameof(value))
        };
}