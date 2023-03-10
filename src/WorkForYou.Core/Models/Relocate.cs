namespace WorkForYou.Core.Models;

public class Relocate
{
    public int RelocateId { get; set; }
    public string RelocateName { get; set; } = string.Empty;
    public IEnumerable<Vacancy>? Vacancies { get; set; }
}
