namespace WorkForYou.Core.Models;

public class Relocate
{
    public int RelocateId { get; set; }
    public string RelocateName { get; set; } = string.Empty;
    public Vacancy? Vacancy { get; set; }
}
