namespace Meetups.WebApi.Swagger;

using Meetups.Persistence.Entities;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

internal class OpenApiMeetupDurationSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type != typeof(Meetup.MeetupDuration))
        {
            return;
        }
        
        schema.Type = "string";
        schema.Pattern = @"^\d+:\d{2}$";
        schema.Example = new OpenApiString("3:00");
    }
}