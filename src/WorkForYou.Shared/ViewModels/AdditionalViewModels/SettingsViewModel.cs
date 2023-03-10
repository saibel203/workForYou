using WorkForYou.Core.AdditionalModels;
using WorkForYou.Core.Models;

namespace WorkForYou.Shared.ViewModels.AdditionalViewModels;

public class SettingsViewModel
{
    public int PageCount { get; set; }
    public int VacancyCount { get; set; }
    public string? Username { get; set; } = string.Empty;
    public string? CurrentController { get; set; } = string.Empty;
    public string? CurrentAction { get; set; } = string.Empty;
    public QueryParameters QueryParameters { get; set; } = new();
    public IEnumerable<int> Pages { get; set; } = new List<int>();
    public int CurrentVacancyId { get; set; }
    public string? ReturnUrl { get; set; } = string.Empty;

    public IReadOnlyList<WorkCategory> WorkCategories { get; set; } = new List<WorkCategory>();
    public IReadOnlyList<EnglishLevel> EnglishLevels { get; set; } = new List<EnglishLevel>();
    public IReadOnlyList<TypesOfCompany> TypesOfCompanies { get; set; } = new List<TypesOfCompany>();
    public IReadOnlyList<HowToWork> HowToWorks { get; set; } = new List<HowToWork>();
    public IReadOnlyList<CandidateRegion> CandidateRegions { get; set; } = new List<CandidateRegion>();

    public IReadOnlyList<CommunicationLanguage> CommunicationLanguages { get; set; } =
        new List<CommunicationLanguage>();
}