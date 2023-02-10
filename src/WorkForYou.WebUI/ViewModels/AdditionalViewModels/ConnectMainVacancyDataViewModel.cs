using WorkForYou.Core.Models;

namespace WorkForYou.WebUI.ViewModels.AdditionalViewModels;

public class ConnectMainVacancyDataViewModel
{
    public IReadOnlyList<VacancyDomain>? VacancyDomains { get; set; } = new List<VacancyDomain>();
    public IReadOnlyList<WorkCategory>? WorkCategories { get; set; } = new List<WorkCategory>();
    public IReadOnlyList<Relocate>? Relocates { get; set; } = new List<Relocate>();
    public IReadOnlyList<EnglishLevel>? EnglishLevels { get; set; } = new List<EnglishLevel>();
    public IReadOnlyList<CandidateRegion>? CandidateRegions { get; set; } = new List<CandidateRegion>();
    public IReadOnlyList<TypesOfCompany>? TypesOfCompanies { get; set; } = new List<TypesOfCompany>();
    public IReadOnlyList<HowToWork>? HowToWorks { get; set; } = new List<HowToWork>();
}
