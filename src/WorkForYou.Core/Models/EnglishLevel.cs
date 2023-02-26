namespace WorkForYou.Core.Models;

public class EnglishLevel
{
    public int EnglishLevelId { get; set; }
    public string NameLevel { get; set; } = string.Empty;
    public string DescriptionLevel { get; set; } = string.Empty;

    public IEnumerable<CandidateUser>? CandidateUsers { get; set; }
    public IEnumerable<Vacancy>? Vacancies { get; set; }
}
