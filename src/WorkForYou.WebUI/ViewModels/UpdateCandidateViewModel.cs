using System.ComponentModel.DataAnnotations;

namespace WorkForYou.WebUI.ViewModels;

public class UpdateCandidateViewModel
{
    [Required(ErrorMessage = "CompanyPosition поле має бути заповненим")]
    [Display(Name = "Посада")]
    [StringLength(100, MinimumLength = 10, ErrorMessage = "Довжина назви має бути в діапазоні від 10 до 100 символів")]
    public string? CompanyPosition { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "ExpectedSalary поле має бути заповненим")]
    [Display(Name = "Зарплатні очікування")]
    public int ExpectedSalary { get; set; }
    
    [Display(Name = "Погодинна ставка")]
    public int HourlyRate { get; set; }
    
    [Display(Name = "Досвід роботи")]
    public int ExperienceWorkTime { get; set; }
    
    [Display(Name = "Опис досвіду роботи")]
    [StringLength(1000, MinimumLength = 50, ErrorMessage = "Спробуйте описати ваш досвід від 50 до 1000 символів")]
    public string? ExperienceWorkDescription { get; set; } = string.Empty;
    
    [Display(Name = "Країна")]
    public string? Country { get; set; } = string.Empty;
    
    [Display(Name = "Місто")]
    public string? City { get; set; } = string.Empty;
    
    [Display(Name = "Варіанти зайнятості")]
    public string? EmploymentOptions { get; set; } = string.Empty;
    
    [Display(Name = "Бажана мова спілкування")]
    public int CommunicationLanguageId { get; set; }
    
    [Display(Name = "Рівень англійської")]
    public int EnglishLevelId { get; set; }
    
    [Display(Name = "Категорія")]
    public int WorkCategoryId { get; set; }
}
