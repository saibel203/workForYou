namespace WorkForYou.Core.Models;

public class WorkCategory
{
    public int WorkCategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;

    public IEnumerable<CandidateUser>? CandidateUsers { get; set; }
    public IEnumerable<Vacancy>? Vacancies { get; set; }
}
