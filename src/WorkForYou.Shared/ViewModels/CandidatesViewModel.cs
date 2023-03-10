using WorkForYou.Core.Models;
using WorkForYou.Core.Models.IdentityInheritance;
using WorkForYou.Shared.ViewModels.AdditionalViewModels;

namespace WorkForYou.Shared.ViewModels;

public class CandidatesViewModel : SettingsViewModel
{
    public IEnumerable<ApplicationUser> ApplicationUsers { get; set; } = new List<ApplicationUser>();
    public IEnumerable<CandidateUser>? CandidateUsers { get; set; } = new List<CandidateUser>();
}