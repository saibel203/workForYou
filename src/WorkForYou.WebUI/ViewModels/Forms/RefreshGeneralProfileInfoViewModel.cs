using System.ComponentModel.DataAnnotations;

namespace WorkForYou.WebUI.ViewModels.Forms;

public class RefreshGeneralProfileInfoViewModel
{
    public string? UserName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "FirstName поле має бути заповненим")]
    [Display(Name = "Ім'я")]
    public string? FirstName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "LastName поле має бути заповненим")]
    [Display(Name = "Призвіще")]
    public string? LastName { get; set; } = string.Empty;
    
    [Display(Name = "Skype")]
    public string? SkypeLink { get; set; } = string.Empty;
    
    [Display(Name = "Github")]
    public string? GithubLink { get; set; } = string.Empty;
    
    [Display(Name = "LinkedIn")]
    public string? LinkedInLink { get; set; } = string.Empty;
    
    [Display(Name = "Telegram")]
    public string? TelegramLink { get; set; } = string.Empty;
}
