using WorkForYou.Core.Models;

namespace WorkForYou.Shared.ViewModels;

public class VacancyViewModel
{
    public Vacancy? Vacancy { get; set; }
    public bool IsVacancyInRespondedList { get; set; }
}
