using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ElectronicShopper.Library.Identity;

/// <summary>
///     Represents a user in the identity system.
/// </summary>
public class ApplicationUser : IdentityUser<int>
{
    /// <summary>
    ///     First name of the user.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;


    /// <summary>
    ///     Last name of the user.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;


    /// <summary>
    ///     UTC time when user was registered.
    /// </summary>
    [Required]
    public DateTime RegistrationTime { get; set; }
}