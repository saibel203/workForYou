using System.ComponentModel.DataAnnotations;

namespace WorkForYou.WebUI.ViewModels;

public class ChangePasswordViewModel
{
    public string Username { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Email поле є обов'язковим")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "OldPassword поле є обов'язвовим")]
    public string OldPassword { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "NewPassword поле є обов'язвовим")]
    [MinLength(6, ErrorMessage = "Мінімальна довжина пароля - 6 символів")]
    [RegularExpression(@"^(?=.*\d)(?=.*[a-zA-Z])(?=.*[A-Z])(?=.*[a-zA-Z]).{6,}$", 
        ErrorMessage = "Пароль має містити принаймні одну цифру, одну велику літеру та одну маленьку літеру")]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "ConfirmNewPassword поле є обов'язковим")]
    [Compare("NewPassword", ErrorMessage = "Паролі не співпадають")]
    [DataType(DataType.Password)]
    public string ConfirmNewPassword { get; set; } = string.Empty;
}
