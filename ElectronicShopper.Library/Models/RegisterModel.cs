using System.ComponentModel.DataAnnotations;
using ElectronicShopper.Library.Identity;
using ElectronicShopper.Library.Validators;

namespace ElectronicShopper.Library.Models;

/// <summary>
///     A model used for creating new <see cref="ApplicationUser" />.
/// </summary>
public class RegisterModel
{
    /// <summary>
    ///     First name of the user.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string? FirstName { get; set; }


    /// <summary>
    ///     Last name of the user.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string? LastName { get; set; }


    /// <summary>
    ///     Email used for creating account.
    /// </summary>
    [Required]
    [MaxLength(50)]
    [EmailValidation]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }


    /// <summary>
    ///     Password used for creating account.
    /// </summary>
    [Required]
    [DataType(DataType.Password)]
    [PasswordValidation]
    public string? Password { get; set; }


    /// <summary>
    ///     A property used for validating whether <see cref="Password" /> was typed in correct.
    ///     In order to successfully validate a model both fields <see cref="Password" /> and <see cref="ConfirmPassword" />
    ///     should have same value.
    /// </summary>
    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(Password))]
    public string? ConfirmPassword { get; set; }
}