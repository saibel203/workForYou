using WorkForYou.Core.DtoModels;
using WorkForYou.Core.Models.IdentityInheritance;
using WorkForYou.Core.Responses.Repositories;

namespace WorkForYou.Core.IRepositories;

public interface IUserRepository : IGenericRepository<ApplicationUser>
{
    Task<UserResponse> GetUserDataAsync(UsernameDto? usernameDto);
    Task<UserResponse> ShowFavouriteListAsync(UsernameDto? usernameDto);
}