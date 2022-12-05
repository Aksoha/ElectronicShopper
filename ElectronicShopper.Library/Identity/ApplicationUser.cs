using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ElectronicShopper.Library.Identity;

public class ApplicationUser : IdentityUser<int>
{

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    public DateTime RegistrationTime { get; set; }
}