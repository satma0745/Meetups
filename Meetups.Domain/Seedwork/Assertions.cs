namespace Meetups.Domain.Seedwork;

using System;

internal static class Assertions
{
    /// <summary>Does nothing.</summary>
    /// <remarks>Just a temporary placeholder to replace in the future (when any validation rules come up).</remarks>
    /// <param name="name">Name of the validated parameter.</param>
    /// <param name="value">Current value of the validated parameter.</param>
    /// <exception cref="DomainValidationException">Thrown if specified value is not valid.</exception>
    public static void None(string name, object value)
    {
    }
    
    /// <summary>Validates any object.</summary>
    /// <remarks>Capable only of performing simple generic validation.</remarks>
    /// <param name="name">Name of the validated parameter.</param>
    /// <param name="value">Current value of the validated parameter.</param>
    /// <param name="required">
    /// Specifies whether parameter is required.
    /// If <c>true</c>, forbids the <c>null</c> value.
    /// </param>
    /// <exception cref="DomainValidationException">Thrown if specified value is not valid.</exception>
    public static void EnsureValidObject(string name, object value, bool required = false)
    {
        if (required && value is null)
        {
            throw new DomainValidationException(name, "must not be empty");
        }
    }
    
    /// <summary>Validates a Guid.</summary>
    /// <param name="name">Name of the validated parameter.</param>
    /// <param name="value">Current value of the validated parameter.</param>
    /// <param name="required">
    /// Specifies whether parameter is required.
    /// If <c>true</c>, forbids the <c>00000000-0000-0000-0000-000000000000</c> value.
    /// </param>
    /// <exception cref="DomainValidationException">Thrown if specified value is not valid.</exception>
    public static void EnsureValidGuid(string name, Guid value, bool required = false)
    {
        if (required && value == Guid.Empty)
        {
            throw new DomainValidationException(name, "must not be empty");
        }
    }

    /// <summary>Validates a number.</summary>
    /// <param name="name">Name of the validated parameter.</param>
    /// <param name="value">Current value of the validated parameter.</param>
    /// <param name="minValue">Lowest acceptable value. Pass <c>null</c> to skip this check.</param>
    /// <param name="maxValue">Highest acceptable value. Pass <c>null</c> to skip this check.</param>
    /// <exception cref="DomainValidationException">Thrown if specified value is not valid.</exception>
    public static void EnsureValidNumber(
        string name,
        int value,
        int? minValue = null,
        int? maxValue = null)
    {
        if (minValue is not null && value < minValue)
        {
            throw new DomainValidationException(name, $"must be greater than or equal to {minValue}");
        }
        if (maxValue is not null && value > maxValue)
        {
            throw new DomainValidationException(name, $"must be less than or equal to {maxValue}");
        }
    }

    /// <summary>Validates a string.</summary>
    /// <param name="name">Name of the validated parameter.</param>
    /// <param name="value">Current value of the validated parameter.</param>
    /// <param name="required">
    /// Specifies whether parameter is required.
    /// If <c>true</c>, forbids the <c>null</c> and <c>""</c> values.
    /// </param>
    /// <param name="minLength">Lowest acceptable string length. Pass <c>null</c> to skip this check.</param>
    /// <param name="maxLength">Highest acceptable string length. Pass <c>null</c> to skip this check.</param>
    /// <param name="exactLength">The only acceptable string length. Pass <c>null</c> to skip this check.</param>
    /// <exception cref="DomainValidationException">Thrown if specified value is not valid.</exception>
    public static void EnsureValidString(
        string name,
        string value,
        bool required = false,
        int? minLength = null,
        int? maxLength = null,
        int? exactLength = null)
    {
        if (required && string.IsNullOrEmpty(value))
        {
            throw new DomainValidationException(name, "must not be null or empty");
        }

        if (minLength is not null && value.Length < minLength)
        {
            throw new DomainValidationException(name, $"must be at least {minLength} characters long");
        }
        if (maxLength is not null && value.Length > maxLength)
        {
            throw new DomainValidationException(name, $"must not exceed {maxLength} characters");
        }
        if (exactLength is not null && value.Length != exactLength)
        {
            throw new DomainValidationException(name, $"must be exactly {exactLength} characters long");
        }
    }
}