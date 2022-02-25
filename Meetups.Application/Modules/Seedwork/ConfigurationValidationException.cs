namespace Meetups.Application.Modules.Seedwork;

using System;

public class ConfigurationValidationException : Exception
{
    public ConfigurationValidationException(string parameterName, string message)
        : base($"Invalid value provided for the \"{parameterName}\" configuration parameter: {message}")
    {
    }
}