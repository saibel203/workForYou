using WorkForYou.Core.Models;

namespace WorkForYou.Core.Responses.Repositories;

public class VacancyResponse : BaseResponse
{
    public IEnumerable<Vacancy> VacancyList { get; set; } = new List<Vacancy>();
    public FavouriteVacancy FavouriteVacancy { get; set; } = new();
    public Vacancy? Vacancy { get; set; }
    public int PageCount { get; set; }
    public int VacancyCount { get; set; }
    public string? SearchString { get; set; }
    public IEnumerable<int> Pages { get; set; } = new List<int>();
}
