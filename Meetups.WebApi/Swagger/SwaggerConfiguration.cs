namespace Meetups.WebApi.Swagger;

using Microsoft.Extensions.Configuration;

public class SwaggerConfiguration
{
    #region IConfiguration support

    public static SwaggerConfiguration FromApplicationConfiguration(IConfiguration configuration) =>
        new SwaggerConfiguration(
            swaggerEnabled: configuration.GetValue<bool?>("Swagger:SwaggerEnabled") ?? false,
            swaggerUiRoutePrefix: configuration.GetValue<string>("Swagger:SwaggerUiRoutePrefix") ?? string.Empty,
            showSchemasInSwaggerUi: configuration.GetValue<bool?>("Swagger:ShowSchemasInSwaggerUi") ?? false);

    #endregion

    #region Parameters

    public bool SwaggerEnabled { get; }
    
    public string SwaggerUiRoutePrefix { get; }
    
    public bool ShowSchemasInSwaggerUi { get; }

    #endregion

    #region Constructors

    public SwaggerConfiguration(bool swaggerEnabled, string swaggerUiRoutePrefix, bool showSchemasInSwaggerUi)
    {
        SwaggerEnabled = swaggerEnabled;
        SwaggerUiRoutePrefix = swaggerUiRoutePrefix;
        ShowSchemasInSwaggerUi = showSchemasInSwaggerUi;
    }

    #endregion
}