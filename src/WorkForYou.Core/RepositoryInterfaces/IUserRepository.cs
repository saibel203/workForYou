using WorkForYou.Core.AdditionalModels;
using WorkForYou.Core.DTOModels.UserDTOs;
using WorkForYou.Core.Models.IdentityInheritance;
using WorkForYou.Core.Responses.Repositories;

namespace WorkForYou.Core.RepositoryInterfaces;

public interface IUserRepository : IGenericRepository<ApplicationUser>
{
    Task<UserResponse> GetUserDataAsync(UsernameDto? usernameDto);
    Task<UserResponse> GetAllCandidatesAsync(QueryParameters queryParameters);
    Task<UserResponse> RefreshGeneralInfoAsync(RefreshGeneralUserDto? refreshGeneralUserDto);
    Task<UserResponse> RefreshCandidateUserInfoAsync(RefreshCandidateDto? refreshCandidateDto);
    Task<UserResponse> RefreshEmployerInfoAsync(RefreshEmployerDto? refreshEmployerDto);
    Task<UserResponse> RemoveUserAsync(UsernameDto? usernameDto);
}