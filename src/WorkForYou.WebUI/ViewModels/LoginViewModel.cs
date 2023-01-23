using System.ComponentModel.DataAnnotations;

namespace WorkForYou.WebUI.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Email поле є обов'язвовим")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Password поле є обов'язвовим")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
    
    public bool RememberMe { get; set; }
}
