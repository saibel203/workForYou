using System.ComponentModel.DataAnnotations;

namespace WorkForYou.WebUI.ViewModels;

public class UpdateEmployerViewModel : BaseUserViewModel
{
    [Required(ErrorMessage = "CompanyName поле має бути заповненим")]
    [Display(Name = "Назва компанії")]
    public string? CompanyName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "CompanyPosition поле має бути заповненим")]
    [Display(Name = "Посада в компанії")]
    public string? CompanyPosition { get; set; } = string.Empty;
    
    [Display(Name = "Веб сайт компанії")]
    public string? CompanySiteLink { get; set; } = string.Empty;
    
    [Display(Name = "Сторінка компанії на DOU")]
    public string? DoyCompanyLink { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "AboutCompany поле має бути заповненим")]
    [Display(Name = "Про компанію")]
    public string? AboutCompany { get; set; } = string.Empty;
}
