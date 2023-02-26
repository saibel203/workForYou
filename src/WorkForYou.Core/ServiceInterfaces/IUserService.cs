using WorkForYou.Core.DTOModels.UserDTOs;
using WorkForYou.Core.Responses.Services;

namespace WorkForYou.Core.ServiceInterfaces;

public interface IUserService
{
    Task<UserResponse> AddCandidateToFavouriteListAsync(UsernameDto? usernameDto, int candidateId);
    Task<UserResponse> IsCandidateInFavouriteListAsync(int employerUserId, int candidateUserId);

}