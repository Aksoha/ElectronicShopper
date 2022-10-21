using System.ComponentModel.DataAnnotations;

namespace ElectronicShopper.Library.Models;

public class LoginModel
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Field can't be empty")]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Field can't be empty")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
    public bool RememberMe { get; set; }
    
}