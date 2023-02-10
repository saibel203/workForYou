using WorkForYou.Core.Models;
using WorkForYou.Core.Models.IdentityInheritance;

namespace WorkForYou.Core.Responses.Repositories;

public class UserResponse : ListBaseResponse
{
    public ApplicationUser User { get; set; } = new();
    public IEnumerable<Vacancy> FavouriteList { get; set; } = new List<Vacancy>();
    public IEnumerable<ApplicationUser> ApplicationUsers { get; set; } = new List<ApplicationUser>();
}
