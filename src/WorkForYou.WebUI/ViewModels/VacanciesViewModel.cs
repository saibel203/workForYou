using WorkForYou.Core.Models;
using WorkForYou.WebUI.ViewModels.AdditionalViewModels;

namespace WorkForYou.WebUI.ViewModels;

public class VacanciesViewModel : SettingsViewModel
{
    public IEnumerable<Vacancy> Vacancies { get; set; } = new List<Vacancy>();
}