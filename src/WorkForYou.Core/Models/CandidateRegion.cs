using System.Collections;

namespace WorkForYou.Core.Models;

public class CandidateRegion
{
    public int CandidateRegionId { get; set; }
    public string CandidateRegionName { get; set; } = string.Empty;
    public IEnumerable<Vacancy>? Vacancies { get; set; }
}
