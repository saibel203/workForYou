namespace WorkForYou.Core.Models;

public class FavouriteVacancy
{
    public int CandidateId { get; set; }
    public CandidateUser? CandidateUser { get; set; }
    public int VacancyId { get; set; }
    public Vacancy? Vacancy { get; set; }
}
