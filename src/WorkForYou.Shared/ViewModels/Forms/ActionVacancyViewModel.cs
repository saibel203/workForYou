using System.ComponentModel.DataAnnotations;
using WorkForYou.Core.Models;
using WorkForYou.Shared.Attributes;
using WorkForYou.Shared.ViewModels.AdditionalViewModels;

namespace WorkForYou.Shared.ViewModels.Forms;

public class ActionVacancyViewModel : ConnectMainVacancyDataViewModel
{
    public int VacancyId { get; set; }

    [Required(ErrorMessage = "VacancyTitle поле має бути заповненим")]
    [Display(Name = "Хто вам потрібен")]
    [StringLength(100, MinimumLength = 10, ErrorMessage = "Довжина назви має бути в діапазоні від 10 до 100 символів")]
    public string VacancyTitle { get; set; } = string.Empty;

    [Required(ErrorMessage = "ShortDescription поле має бути заповненим")]
    [Display(Name = "Короткий опис")]
    [StringLength(250, MinimumLength = 30,
        ErrorMessage = "Довжина короткого опису має бути в діапазоні від 30 до 250 символів")]
    public string ShortDescription { get; set; } = string.Empty;

    [Required(ErrorMessage = "LongDescription поле має бути заповненим")]
    [Display(Name = "Детальний опис")]
    public string LongDescription { get; set; } = string.Empty;

    [Required(ErrorMessage = "FromSalary поле має бути заповненим")]
    [Display(Name = "Зарплата від")]
    [CheckModelSalary("ToSalary", ErrorMessage = "Мінімальна зарплата не може перевищувати максимальну")]
    public int? FromSalary { get; set; }

    [Required(ErrorMessage = "ToSalary поле має бути заповненим")]
    [Display(Name = "Зарплата до")]
    public int? ToSalary { get; set; }

    [Required(ErrorMessage = "KeyWords поле має бути заповненим (бажано > 100 символів)")]
    [Display(Name = "Ключові слова для пошуку")]
    [StringLength(300, MinimumLength = 10,
        ErrorMessage = "Довжина ключових слів має бути в діапазоні від 10 до 300 символів")]
    public string KeyWords { get; set; } = string.Empty;

    [Required(ErrorMessage = "Experience поле має бути заповненим")]
    [Display(Name = "Досвід роботи")]
    [Range(0, 15, ErrorMessage = "Досвід повинен сягати від 0 до 15 років")]
    public int? ExperienceWork { get; set; }

    [Required(ErrorMessage = "VacancyDomain поле має бути заповненим")]
    [Display(Name = "Домен вакансії")]
    public int VacancyDomainId { get; set; }

    [Required(ErrorMessage = "WorkCategory поле має бути заповненим")]
    [Display(Name = "Категорія")]
    public int WorkCategoryId { get; set; }

    [Required(ErrorMessage = "HowToWork поле має бути заповненим")]
    [Display(Name = "Ремоут / офіс")]
    public int HowToWorkId { get; set; }

    [Required(ErrorMessage = "Relocate поле має бути заповненим")]
    [Display(Name = "Релокейт")]
    public int RelocateId { get; set; }

    [Required(ErrorMessage = "CandidateRegion поле має бути заповненим")]
    [Display(Name = "Регіон кандидата")]
    public int CandidateRegionId { get; set; }

    [Required(ErrorMessage = "EnglishLevel поле має бути заповненим")]
    [Display(Name = "Рівень англійської")]
    public int EnglishLevelId { get; set; }

    [Required(ErrorMessage = "TypesOfCompany поле має бути заповненим")]
    [Display(Name = "Тип компанії")]
    public int TypesOfCompanyId { get; set; }

    public EmployerUser? EmployerUser { get; set; }
}