namespace Meetups.Backend.Features.Shared;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class OneOfAttribute : ValidationAttribute
{
    public IReadOnlyCollection<object> AllowedValues => allowedValues;

    private readonly object[] allowedValues;

    public OneOfAttribute(Type enumerationType)
    {
        var isStatic = enumerationType.IsAbstract && enumerationType.IsSealed;
        if (!isStatic && enumerationType.IsClass)
        {
            throw new ArgumentException("Only static classes are supported.", nameof(enumerationType));
        }

        allowedValues = enumerationType
            .GetFields()
            .Select(field => field.GetValue(null))
            .ToArray();
    }

    public override bool IsValid(object value) =>
        allowedValues.Contains(value);

    public override string FormatErrorMessage(string name)
    {
        const string template = "The value of the {0} field is not allowed. Options are: [{1}].";
        var serializedAllowedValues = string.Join(", ", allowedValues.Select(value => $"\"{value}\""));
        return string.Format(template, name, serializedAllowedValues);
    }
}