using WorkForYou.Core.Models;
using WorkForYou.Shared.ViewModels.AdditionalViewModels;

namespace WorkForYou.Shared.ViewModels;

public class VacanciesViewModel : SettingsViewModel
{
    public IEnumerable<Vacancy> Vacancies { get; set; } = new List<Vacancy>();
}