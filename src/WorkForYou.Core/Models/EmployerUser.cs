namespace WorkForYou.Core.Models;

public class EmployerUser : BaseUser
{
    public int EmployerUserId { get; set; }
    public string? CompanyPosition { get; set; } = string.Empty;
    public string? CompanyName { get; set; } = string.Empty;
    public string? CompanySiteLink { get; set; } = string.Empty;
    public string? DoyCompanyLink { get; set; } = string.Empty;
    public string? AboutCompany { get; set; } = string.Empty;
    public IEnumerable<Vacancy>? Vacancies { get; set; } = new List<Vacancy>();
    public IEnumerable<FavouriteCandidate>? FavouriteCandidates { get; set; }
}
