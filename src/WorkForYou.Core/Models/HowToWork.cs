using System.Collections;

namespace WorkForYou.Core.Models;

public class HowToWork
{
    public int HowToWorkId { get; set; }
    public string HowToWorkName { get; set; } = string.Empty;
    public IEnumerable<Vacancy>? Vacancies { get; set; }
}
