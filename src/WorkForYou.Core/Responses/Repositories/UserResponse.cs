using WorkForYou.Core.Models;
using WorkForYou.Core.Models.IdentityInheritance;

namespace WorkForYou.Core.Responses.Repositories;

public class UserResponse : BaseResponse
{
    public ApplicationUser User { get; set; } = new();
    public IEnumerable<Vacancy?> FavouriteList { get; set; } = new List<Vacancy>();
    public IEnumerable<Vacancy> EmployerVacancyList { get; set; } = new List<Vacancy>();
}
