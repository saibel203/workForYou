namespace WorkForYou.Data.Models;

public class CandidateUser : BaseUser
{
    public int CandidateUserId { get; set; }
    public string? CompanyPosition { get; set; } = string.Empty;
    public int ExpectedSalary { get; set; }
    public int HourlyRate { get; set; }
    public int ExperienceWorkTime { get; set; }
    public string? ExperienceWorkDescription { get; set; } = string.Empty;
    public string? Country { get; set; } = string.Empty;
    public string? City { get; set; } = string.Empty;
    public string? EmploymentOptions { get; set; } = string.Empty;

    public int? EnglishLevelId { get; set; }
    public EnglishLevel? LevelEnglish { get; set; }

    public int? WorkCategoryId { get; set; }
    public WorkCategory? CategoryWork { get; set; }
}
