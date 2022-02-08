namespace Meetups.Backend.WebApi.EntryPoint;

using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

internal static class Program
{
    private static void Main(string[] arguments) =>
        CreateHostBuilder(arguments).Build().Run();

    private static IHostBuilder CreateHostBuilder(string[] arguments) =>
        Host.CreateDefaultBuilder(arguments)
            .ConfigureAppConfiguration(ConfigureAppSettings)
            .ConfigureWebHostDefaults(webHost => webHost.UseStartup<Startup>());

    private static void ConfigureAppSettings(HostBuilderContext host, IConfigurationBuilder configuration)
    {
        var baseConfig = Path.Combine("Properties", "appSettings.json");
        configuration.AddJsonFile(baseConfig, optional: true);

        var environmentName = host.HostingEnvironment.EnvironmentName;
        var environmentSpecificConfig = Path.Combine("Properties", $"appSettings.{environmentName}.json");
        configuration.AddJsonFile(environmentSpecificConfig, optional: true);
    }
}
