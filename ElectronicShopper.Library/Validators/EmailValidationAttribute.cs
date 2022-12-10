using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ElectronicShopper.Library.Validators;

/// <summary>
///     A rule validating whether string is an email.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public partial class EmailValidationAttribute : ValidationAttribute
{
    public EmailValidationAttribute()
    {
        ErrorMessage = "Input is not an email";
    }

    public override bool IsValid(object? value)
    {
        return value is string strValue && EmailPattern().IsMatch(strValue);
    }

    [GeneratedRegex(
        "[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+" +
        "[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", RegexOptions.IgnoreCase)]
    private static partial Regex EmailPattern();
}