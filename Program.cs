using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Meetups.Context;
using Meetups.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

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
    
    // Swagger JWT auth support
    options.AddSecurityDefinition("Bearer", new()
    {
        Description = "Put Your access token here:",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
    options.OperationFilter<OpenApiAuthFilter>();
    
    // Use xml comments from the source code
    var projectName = Assembly.GetExecutingAssembly().GetName().Name;
    var rootDirectory = AppContext.BaseDirectory;
    var pathToCommentsFile = Path.Combine(rootDirectory, $"{projectName}.xml");
    options.IncludeXmlComments(pathToCommentsFile);
});
builder.Services.AddDbContext<ApplicationContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("PostgreSQL");
    options.UseNpgsql(connectionString);
});
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var rawSigningKey = builder.Configuration["Auth:SecretKey"];
        var signingKeyBytes = Encoding.ASCII.GetBytes(rawSigningKey);

        options.TokenValidationParameters = new()
        {
            RequireSignedTokens = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes),

            ValidateAudience = false,
            ValidateIssuer = false,

            RequireExpirationTime = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
        options.RequireHttpsMetadata = false;
        
        var tokenHandler = options.SecurityTokenValidators
            .OfType<JwtSecurityTokenHandler>()
            .Single();
        tokenHandler.InboundClaimTypeMap.Clear();
        tokenHandler.OutboundClaimTypeMap.Clear();
    });

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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
