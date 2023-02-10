using WorkForYou.Core.Models;

namespace WorkForYou.Core.Responses.Repositories;

public class VacancyResponse : ListBaseResponse
{
    public IEnumerable<Vacancy> VacancyList { get; set; } = new List<Vacancy>();
    public FavouriteVacancy FavouriteVacancy { get; set; } = new();
    public Vacancy? Vacancy { get; set; }
}
