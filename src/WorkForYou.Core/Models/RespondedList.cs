using WorkForYou.Core.Models.IdentityInheritance;

namespace WorkForYou.Core.Models;

public class RespondedList
{
    public string ApplicationUserId { get; set; } = string.Empty;
    public ApplicationUser? ApplicationUser { get; set; }
    public int VacancyId { get; set; }
    public Vacancy? Vacancy { get; set; }
}
