using System.ComponentModel.DataAnnotations;

namespace WorkForYou.WebUI.ViewModels.Forms;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Username поле є обов'язвовим")] 
    [StringLength(20, MinimumLength = 6, ErrorMessage = "Довжина Username має бути від 6 до 20 символів")]
    public string Username { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Email поле є обов'язвовим")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Password поле є обов'язвовим")]
    [MinLength(6, ErrorMessage = "Мінімальна довжина пароля - 6 символів")]
    [RegularExpression(@"^(?=.*\d)(?=.*[a-zA-Z])(?=.*[A-Z])(?=.*[a-zA-Z]).{6,}$", 
        ErrorMessage = "Пароль має містити принаймні одну цифру, одну велику літеру та одну маленьку літеру")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "ConfirmPassword поле є обов'язковим")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Паролі не співпадають")]
    public string ConfirmPassword { get; set; } = string.Empty;
    
    [Required (ErrorMessage = "SelectedRole поле є обов'язвовим")] 
    public string SelectedRole { get; set; } = string.Empty;

    public string ReturnUrl { get; set; } = string.Empty;
}
