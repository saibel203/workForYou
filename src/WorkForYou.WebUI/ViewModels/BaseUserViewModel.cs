using System.ComponentModel.DataAnnotations;

namespace WorkForYou.WebUI.ViewModels;

public class BaseUserViewModel
{
    [Display(Name = "Ім'я")]
    public string? FirstName { get; set; } = string.Empty;
    
    [Display(Name = "Прізвище")]
    public string? LastName { get; set; } = string.Empty;
    
    [Display(Name = "Skype посилання")]
    public string? SkypeLink { get; set; } = string.Empty;
    
    [Display(Name = "GitHub посилання")]
    public string? GithubLink { get; set; } = string.Empty;
    
    [Display(Name = "LinkedIn посилання")]
    public string? LinkedInLink { get; set; } = string.Empty;
    
    [Display(Name = "Telegram посилання")]
    public string? TelegramLink { get; set; } = string.Empty;
    
    [Display(Name = "Завантажити нове фото")]
    public string? ImagePath { get; set; } = string.Empty;
}
