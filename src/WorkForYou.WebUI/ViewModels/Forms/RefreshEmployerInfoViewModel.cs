using System.ComponentModel.DataAnnotations;

namespace WorkForYou.WebUI.ViewModels.Forms;

public class RefreshEmployerInfoViewModel
{
    [Required(ErrorMessage = "CompanyPosition поле має бути заповненим")]
    [Display(Name = "Позиція в компанії")]
    public string? CompanyPosition { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "CompanyName поле має бути заповненим")]
    [Display(Name = "Назва компанії")]
    public string? CompanyName { get; set; } = string.Empty;
    
    [Display(Name = "Сайт компанії")]
    public string? CompanySiteLink { get; set; } = string.Empty;
    
    [Display(Name = "DOY посилання")]
    public string? DoyCompanyLink { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "AboutCompany поле має бути заповненим")]
    [Display(Name = "Про компанію")]
    public string? AboutCompany { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;
}
