using Microsoft.AspNetCore.Http;
using WorkForYou.Core.AdditionalModels;
using WorkForYou.Core.DTOModels.UserDTOs;
using WorkForYou.Core.Models.IdentityInheritance;
using WorkForYou.Core.Responses.Repositories;

namespace WorkForYou.Core.IRepositories;

public interface IUserRepository : IGenericRepository<ApplicationUser>
{
    Task<UserResponse> GetUserDataAsync(UsernameDto? usernameDto);
    Task<UserResponse> ShowFavouriteListAsync(UsernameDto? usernameDto, QueryParameters queryParameters);
    Task<UserResponse> ShowFavouriteCandidatesListAsync(UsernameDto? usernameDto, QueryParameters queryParameters);
    Task<UserResponse> GetAllCandidatesAsync(QueryParameters queryParameters);
    Task<UserResponse> UploadUserImageAsync(IFormFile image, UsernameDto? usernameDto);
    Task<UserResponse> RefreshGeneralInfoAsync(RefreshGeneralUserDto? refreshGeneralUserDto);
    Task<UserResponse> RefreshCandidateUserInfoAsync(RefreshCandidateDto? refreshCandidateDto);
    Task<UserResponse> RefreshEmployerInfoAsync(RefreshEmployerDto? refreshEmployerDto);
    Task<UserResponse> RemoveUserAsync(UsernameDto? usernameDto);
}