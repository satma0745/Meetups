namespace Meetups.Features.Shared;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
internal class OneOfAttribute : ValidationAttribute
{
    public IReadOnlyCollection<object> AllowedValues => allowedValues;

    private readonly object[] allowedValues;

    public OneOfAttribute(params object[] allowedValues) =>
        this.allowedValues = allowedValues;

    public override bool IsValid(object value) =>
        allowedValues.Contains(value);

    public override string FormatErrorMessage(string name)
    {
        const string template = "The value of the {0} field is not allowed. Options are: [{1}].";
        var serializedAllowedValues = string.Join(", ", allowedValues.Select(value => $"\"{value}\""));
        return string.Format(template, name, serializedAllowedValues);
    }
}