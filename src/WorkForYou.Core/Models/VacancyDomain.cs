namespace WorkForYou.Core.Models;

public class VacancyDomain
{
    public int VacancyDomainId { get; set; }
    public string VacancyDomainName { get; set; } = string.Empty;
    public IEnumerable<Vacancy>? Vacancies { get; set; }
}
