using System.ComponentModel.DataAnnotations;
using ElectronicShopper.Library.Validators;

namespace ElectronicShopper.Library.Models;

public class RegisterModel
{
    [Required] [MaxLength(50)] public string? FirstName { get; set; }

    [Required] [MaxLength(50)] public string? LastName { get; set; }


    [Required]
    [MaxLength(50)]
    [EmailValidation]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }


    [Required]
    [DataType(DataType.Password)]
    [PasswordValidation]
    public string? Password { get; set; }


    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(Password))]
    public string? ConfirmPassword { get; set; }
}