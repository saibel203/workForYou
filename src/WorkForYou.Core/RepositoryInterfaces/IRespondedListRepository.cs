using WorkForYou.Core.AdditionalModels;
using WorkForYou.Core.DTOModels.UserDTOs;
using WorkForYou.Core.Responses.Repositories;

namespace WorkForYou.Core.RepositoryInterfaces;

public interface IRespondedListRepository
{
    Task<RespondedListResponse> RespondToVacancyAsync(UsernameDto? usernameDto, int vacancyId);
    Task<RespondedListResponse> RemoveRespondToVacancyAsync(UsernameDto? usernameDto, int vacancyId);
    Task<VacancyResponse> AllCandidateRespondedAsync(UsernameDto? usernameDto, QueryParameters queryParameters);
    Task<RespondedListResponse> IsVacancyInRespondedListAsync(string userId, int vacancyId);
    Task<UserResponse> AllVacancyResponses(int vacancyId, QueryParameters queryParameters);
}