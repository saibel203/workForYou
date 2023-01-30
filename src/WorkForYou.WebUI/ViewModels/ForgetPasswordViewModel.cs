using System.ComponentModel.DataAnnotations;

namespace WorkForYou.WebUI.ViewModels;

public class ForgetPasswordViewModel
{
    [Required (ErrorMessage = "Email поле є обов'язвовим")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}
