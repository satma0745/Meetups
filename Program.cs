using System;
using System.IO;
using System.Reflection;
using Meetups.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var apiVersion = "v1";

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Brief API introduction information
    options.SwaggerDoc(apiVersion, new()
    {
        Version = apiVersion,
        Title = "Meetups API",
        Description = "Meetup CRUD (hopefully not only CRUD) ASP .Net Core Web API",
        Contact = new()
        {
            Name = "Satma0745",
            Url = new Uri("https://t.me/satma0745")
        }
    });
    
    // Use xml comments from the source code
    var projectName = Assembly.GetExecutingAssembly().GetName().Name;
    var rootDirectory = AppContext.BaseDirectory;
    var pathToCommentsFile = Path.Combine(rootDirectory, $"{projectName}.xml");
    options.IncludeXmlComments(pathToCommentsFile);
});
builder.Services.AddDbContext<ApplicationContext>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        // Hide "Schemas" section
        options.DefaultModelsExpandDepth(-1);
        
        // Serve Swagger UI on the "/api" url
        options.SwaggerEndpoint($"/swagger/{apiVersion}/swagger.json", apiVersion);
        options.RoutePrefix = "api";
    });
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
