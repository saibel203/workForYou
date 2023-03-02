namespace WorkForYou.Core.Models;

public class CandidateUser : BaseUser
{
    public int CandidateUserId { get; set; }
    public int ExpectedSalary { get; set; }
    public int HourlyRate { get; set; }
    public int ExperienceWorkTime { get; set; }
    public string? ExperienceWorkDescription { get; set; } = string.Empty;
    public string? Country { get; set; } = string.Empty;
    public string? City { get; set; } = string.Empty;
    public string? EmploymentOptions { get; set; } = string.Empty;
    public string? KeyWords { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public bool IsProfileComplete { get; set; }
    public int ViewCount { get; set; }
    public string? CompanyPosition { get; set; } = string.Empty;
    
    public int? CommunicationLanguageId { get; set; }
    public CommunicationLanguage? CommunicationLanguage { get; set; }

    public int? EnglishLevelId { get; set; }
    public EnglishLevel? LevelEnglish { get; set; }

    public int? WorkCategoryId { get; set; }
    public WorkCategory? CategoryWork { get; set; }
    
    public IEnumerable<FavouriteVacancy>? FavouriteVacancyCollection { get; set; }
    public IEnumerable<FavouriteCandidate>? FavouriteCandidates { get; set; }
}
