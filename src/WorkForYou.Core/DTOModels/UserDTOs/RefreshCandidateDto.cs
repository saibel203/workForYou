namespace WorkForYou.Core.DTOModels.UserDTOs;

public class RefreshCandidateDto
{
    public string Username { get; set; } = string.Empty;
    public int ExpectedSalary { get; set; }
    public int HourlyRate { get; set; }
    public int ExperienceWorkTime { get; set; }
    public string? ExperienceWorkDescription { get; set; } = string.Empty;
    public string? Country { get; set; } = string.Empty;
    public string? City { get; set; } = string.Empty;
    public string? EmploymentOptions { get; set; } = string.Empty;
    public string? KeyWords { get; set; } = string.Empty;
    public bool IsProfileComplete { get; set; }
    public string? CompanyPosition { get; set; } = string.Empty;
    public int? CommunicationLanguageId { get; set; }
    public int? EnglishLevelId { get; set; }
    public int? WorkCategoryId { get; set; }
}
