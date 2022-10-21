using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ElectronicShopper.Library.Validators;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class EmailValidationAttribute : ValidationAttribute
{
    public EmailValidationAttribute()
    {
        ErrorMessage = "Input is not an email";
    }

    public override bool IsValid(object? value)
    {
        if (value is not string strValue)
            return false;
        
        const string pattern =
            @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
        return Regex.IsMatch(strValue, pattern, RegexOptions.IgnoreCase);
    }
}