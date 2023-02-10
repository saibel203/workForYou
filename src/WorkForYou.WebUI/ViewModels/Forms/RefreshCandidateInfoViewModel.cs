using System.ComponentModel.DataAnnotations;
using WorkForYou.WebUI.ViewModels.AdditionalViewModels;

namespace WorkForYou.WebUI.ViewModels.Forms;

public class RefreshCandidateInfoViewModel : ConnectMainUsersDataViewmodel
{
    [Required(ErrorMessage = "ExpectedSalary поле має бути заповненим")]
    [Display(Name = "Зарплатні очікування")]
    public int ExpectedSalary { get; set; }
    
    [Display(Name = "Погодинна ставка")]
    public int HourlyRate { get; set; }
    
    [Required(ErrorMessage = "ExperienceWorkTime поле має бути заповненим")]
    [Display(Name = "Досвід роботи")]
    public int ExperienceWorkTime { get; set; }
    
    [Required(ErrorMessage = "ExperienceWorkDescription поле має бути заповненим")]
    [Display(Name = "Опис досвіду роботи")]
    [StringLength(500, MinimumLength = 50,
        ErrorMessage = "Довжина опису має сягати від 50 до 500 символів")]
    public string? ExperienceWorkDescription { get; set; } = string.Empty;
    
    [Display(Name = "Країна")]
    public string? Country { get; set; } = string.Empty;
    
    [Display(Name = "Місто")]
    public string? City { get; set; } = string.Empty;
    
    [Display(Name = "Варіанти зайнятості")]
    public string? EmploymentOptions { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "KeyWords поле має бути заповненим")]
    [Display(Name = "Ключові слова")]
    [StringLength(300, MinimumLength = 10,
        ErrorMessage = "Довжина ключових слів має бути в діапазоні від 10 до 300 символів")]
    public string? KeyWords { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "CompanyPosition поле має бути заповненим")]
    [Display(Name = "Посада")]
    public string? CompanyPosition { get; set; } = string.Empty;
    
    [Display(Name = "Бажана мова спілкування")]
    public int? CommunicationLanguageId { get; set; }
    
    [Display(Name = "Рівень англійської")]
    public int? EnglishLevelId { get; set; }
    
    [Required(ErrorMessage = "WorkCategory поле має бути заповненим")]
    [Display(Name = "Категорія")]
    public int? WorkCategoryId { get; set; }
    public string Username { get; set; } = string.Empty;
}
