using WorkForYou.Core.AdditionalModels;

namespace WorkForYou.WebUI.ViewModels.AdditionalViewModels;

public class SettingsViewModel
{
    public int PageCount { get; set; }
    public int VacancyCount { get; set; }
    public string? Username { get; set; } = string.Empty;
    public string? CurrentController { get; set; } = string.Empty;
    public string? CurrentAction { get; set; } = string.Empty;
    public QueryParameters QueryParameters { get; set; } = new();
    public IEnumerable<int> Pages { get; set; } = new List<int>();
}