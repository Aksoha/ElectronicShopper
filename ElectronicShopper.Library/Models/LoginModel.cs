using System.ComponentModel.DataAnnotations;
using ElectronicShopper.Library.Identity;

namespace ElectronicShopper.Library.Models;

/// <summary>
///     A model used for authentication of <see cref="ApplicationUser" />.
/// </summary>
public class LoginModel
{
    /// <summary>
    ///     An email of a user that attempting to login.
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = "Field can't be empty")]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }


    /// <summary>
    ///     A password of a user that is attempting to login.
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = "Field can't be empty")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    /// <summary>
    ///     Indicates whether sign-in cookie should persist after the browser is closed.
    /// </summary>
    public bool RememberMe { get; set; }
}