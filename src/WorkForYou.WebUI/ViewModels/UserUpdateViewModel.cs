using WorkForYou.Core.Models;

namespace WorkForYou.WebUI.ViewModels;

public class UserUpdateViewModel
{
    public UpdateCandidateViewModel? UpdateCandidate { get; set; }
    public UpdateEmployerViewModel? UpdateEmployer { get; set; }
    public IEnumerable<VacancyDomain>? VacancyDomains { get; set; } = new List<VacancyDomain>();
    public IEnumerable<WorkCategory>? WorkCategories { get; set; } = new List<WorkCategory>();
    public IEnumerable<Relocate>? Relocates { get; set; } = new List<Relocate>();
    public IEnumerable<EnglishLevel>? EnglishLevels { get; set; } = new List<EnglishLevel>();
    public IEnumerable<CandidateRegion>? CandidateRegions { get; set; } = new List<CandidateRegion>();
    public IEnumerable<TypesOfCompany>? TypesOfCompanies { get; set; } = new List<TypesOfCompany>();
    public IEnumerable<HowToWork>? HowToWorks { get; set; } = new List<HowToWork>();
}
