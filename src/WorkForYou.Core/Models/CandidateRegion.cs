namespace WorkForYou.Core.Models;

public class CandidateRegion
{
    public int CandidateRegionId { get; set; }
    public string CandidateRegionName { get; set; } = string.Empty;
    public ICollection<Vacancy>? Vacancies { get; set; }
}
