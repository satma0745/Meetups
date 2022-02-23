namespace Meetups.Backend.Persistence.Context;

using System;
using Microsoft.Extensions.Configuration;

public class PersistenceConfiguration
{
    #region IConfiguration support

    public static PersistenceConfiguration FromApplicationConfiguration(IConfiguration configuration)
    {
        const string hostPath = "Persistence:Host";
        var host = configuration.GetValue<string>(hostPath);
        if (string.IsNullOrWhiteSpace(host))
        {
            throw ValidationException(hostPath, "parameter is required");
        }

        const string portPath = "Persistence:Port";
        var port = configuration.GetValue<int?>(portPath);
        if (port is null)
        {
            throw ValidationException(portPath, "parameter is required");
        }

        const string databasePath = "Persistence:Database";
        var database = configuration.GetValue<string>(databasePath);
        if (string.IsNullOrWhiteSpace(database))
        {
            throw ValidationException(databasePath, "parameter is required");
        }

        const string usernamePath = "Persistence:Username";
        var username = configuration.GetValue<string>(usernamePath);
        if (string.IsNullOrWhiteSpace(username))
        {
            throw ValidationException(usernamePath, "parameter is required");
        }

        const string passwordPath = "Persistence:Password";
        var password = configuration.GetValue<string>(passwordPath);
        if (string.IsNullOrWhiteSpace(password))
        {
            throw ValidationException(passwordPath, "parameter is required");
        }

        return new PersistenceConfiguration(host, port.Value, database, username, password);
    }
    
    private static Exception ValidationException(string path, string message) =>
        throw new($"Invalid value provided for the \"{path}\" configuration parameter: {message}.");
    
    #endregion
    
    #region Parameters
    public string ConnectionString { get; }

    #endregion
    
    #region Constructors

    private PersistenceConfiguration(string host, int port, string database, string username, string password) =>
        ConnectionString = $"Server={host};Port={port};Database={database};User Id={username};Password={password};";
    
    #endregion
}