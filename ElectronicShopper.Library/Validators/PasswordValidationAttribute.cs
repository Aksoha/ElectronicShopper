using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ElectronicShopper.Library.Validators;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class PasswordValidationAttribute : ValidationAttribute
{
    public PasswordValidationAttribute()
    {
        ErrorMessage =
            "Password must contain an uppercase character, lowercase character, a digit, and a non-alphanumeric character and be at least 8 characters long.";
    }

    public override bool IsValid(object? value)
    {
        var strValue = value as string;
        if (string.IsNullOrWhiteSpace(strValue))
            return false;

        const string pattern = @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9]).{8,}$";
        return Regex.IsMatch(strValue, pattern);
    }
}