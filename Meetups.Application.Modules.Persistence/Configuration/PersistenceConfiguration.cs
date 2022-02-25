namespace Meetups.Application.Modules.Persistence.Configuration;

using Meetups.Application.Modules.Seedwork;
using Microsoft.Extensions.Configuration;

internal class PersistenceConfiguration : IPersistenceConfiguration
{
    #region IConfiguration support

    public static PersistenceConfiguration FromApplicationConfiguration(IConfiguration configuration)
    {
        const string hostPath = "Persistence:Host";
        var host = configuration
            .GetValue<string>(hostPath)
            .Required(hostPath);

        const string portPath = "Persistence:Port";
        var port = configuration
            .GetValue<int?>(portPath)
            .Required(portPath);

        const string databasePath = "Persistence:Database";
        var database = configuration
            .GetValue<string>(databasePath)
            .Required(databasePath);

        const string usernamePath = "Persistence:Username";
        var username = configuration
            .GetValue<string>(usernamePath)
            .Required(usernamePath);

        const string passwordPath = "Persistence:Password";
        var password = configuration
            .GetValue<string>(passwordPath)
            .Required(passwordPath);

        return new PersistenceConfiguration(host, port, database, username, password);
    }
    
    #endregion
    
    #region Parameters
    
    public string ConnectionString { get; }

    #endregion
    
    #region Constructors

    private PersistenceConfiguration(string host, int port, string database, string username, string password) =>
        ConnectionString = $"Server={host};Port={port};Database={database};User Id={username};Password={password};";
    
    #endregion
}