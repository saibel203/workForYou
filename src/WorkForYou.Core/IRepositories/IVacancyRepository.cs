using WorkForYou.Core.AdditionalModels;
using WorkForYou.Core.DTOModels.UserDTOs;
using WorkForYou.Core.DTOModels.VacancyDTOs;
using WorkForYou.Core.Models;
using WorkForYou.Core.Responses.Repositories;

namespace WorkForYou.Core.IRepositories;

public interface IVacancyRepository : IGenericRepository<Vacancy>
{
    Task<VacancyResponse> GetAllVacanciesAsync(QueryParameters queryParameters);
    Task<VacancyResponse> GetVacancyByIdAsync(int id);
    Task<VacancyResponse> CreateVacancyAsync(ActionVacancyDto? actionVacancyDto);
    Task<VacancyResponse> UpdateVacancyAsync(ActionVacancyDto? actionVacancyDto);
    Task<VacancyResponse> RemoveVacancyAsync(int vacancyId);

    Task<VacancyResponse> GetAllEmployerVacanciesAsync(UsernameDto? usernameDto, QueryParameters queryParameters);
}