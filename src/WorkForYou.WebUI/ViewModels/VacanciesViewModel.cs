using WorkForYou.Core.Models;

namespace WorkForYou.WebUI.ViewModels;

public class VacanciesViewModel
{
    public int PageNumber { get; set; }
    public int PageCount { get; set; }
    public int VacancyCount { get; set; }
    public string? SearchString { get; set; } = string.Empty;
    public string? CurrentController { get; set; } = string.Empty;
    public IEnumerable<int> Pages { get; set; } = new List<int>();
    public IEnumerable<Vacancy> Vacancies { get; set; } = new List<Vacancy>();
}
