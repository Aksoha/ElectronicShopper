using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ElectronicShopper.Library.Validators;

/// <summary>
///     A rule validating whether string will be accepted by the database as a password.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public partial class PasswordValidationAttribute : ValidationAttribute
{
    public PasswordValidationAttribute()
    {
        ErrorMessage =
            "Password must contain an uppercase character, lowercase character, a digit, and a non-alphanumeric character and be at least 8 characters long.";
    }

    public override bool IsValid(object? value)
    {
        var strValue = value as string;
        return !string.IsNullOrWhiteSpace(strValue) && PasswordPattern().IsMatch(strValue);
    }

    [GeneratedRegex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9]).{8,}$")]
    private static partial Regex PasswordPattern();
}