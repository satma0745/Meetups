namespace Meetups.Domain.Seedwork;

using System;

internal class DomainValidationException : Exception
{
    public DomainValidationException(string parameterName, string message)
        : base($"Invalid value for the {parameterName} field: {message}.")
    {
    }
}