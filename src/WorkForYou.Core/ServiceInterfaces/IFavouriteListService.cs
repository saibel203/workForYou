using WorkForYou.Core.AdditionalModels;
using WorkForYou.Core.DTOModels.UserDTOs;
using WorkForYou.Core.Responses.Services;

namespace WorkForYou.Core.ServiceInterfaces;

public interface IFavouriteListService
{
    Task<FavouriteListResponse> GetFavouriteVacancyListAsync(UsernameDto? usernameDto,
        QueryParameters queryParameters);

    Task<FavouriteListResponse> GetFavouriteCandidateListAsync(UsernameDto? usernameDto,
        QueryParameters queryParameters);

    Task<FavouriteListResponse> AddCandidateToFavouriteListAsync(UsernameDto? usernameDto, int candidateId);

    Task<FavouriteListResponse> IsCandidateInFavouriteListAsync(int employerUserId, int candidateUserId);

    Task<FavouriteListResponse> AddVacancyToFavouriteListAsync(UsernameDto? usernameDto, int vacancyId);
    
    Task<FavouriteListResponse> IsVacancyInFavouriteListAsync(int vacancyId, int candidateId);
}