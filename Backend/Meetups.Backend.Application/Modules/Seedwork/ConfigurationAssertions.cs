namespace Meetups.Backend.Application.Modules.Seedwork;

public static class ConfigurationAssertions
{
    public static int Required(this int? value, string parameterName) =>
        value ?? throw new ConfigurationValidationException(parameterName, "parameter is required");

    public static int EnsurePositive(this int value, string parameterName) =>
        value < 0
            ? throw new ConfigurationValidationException(parameterName, "must be a positive number")
            : value;

    public static string Required(this string value, string parameterName) =>
        string.IsNullOrEmpty(value)
            ? throw new ConfigurationValidationException(parameterName, "parameter is required")
            : value;
}