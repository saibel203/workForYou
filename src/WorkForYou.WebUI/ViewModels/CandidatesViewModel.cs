using WorkForYou.Core.Models.IdentityInheritance;
using WorkForYou.WebUI.ViewModels.AdditionalViewModels;

namespace WorkForYou.WebUI.ViewModels;

public class CandidatesViewModel : SettingsViewModel
{
    public IEnumerable<ApplicationUser> ApplicationUsers { get; set; } = new List<ApplicationUser>();
}