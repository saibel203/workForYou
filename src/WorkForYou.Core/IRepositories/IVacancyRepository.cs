using WorkForYou.Core.DtoModels;
using WorkForYou.Core.Models;
using WorkForYou.Core.Responses.Repositories;

namespace WorkForYou.Core.IRepositories;

public interface IVacancyRepository : IGenericRepository<Vacancy>
{
    Task<VacancyResponse> GetAllVacanciesAsync(int pageNumber, string? searchString);
    Task<VacancyResponse> GetVacancyByIdAsync(int id);
    Task<VacancyResponse> CreateVacancyAsync(ActionVacancyDto? actionVacancyDto);
    Task<VacancyResponse> UpdateVacancyAsync(ActionVacancyDto? actionVacancyDto);
    Task<VacancyResponse> RemoveVacancyAsync(int vacancyId);
    Task<VacancyResponse> AddVacancyToFavouriteList(string username, int vacancyId);
    Task<VacancyResponse> GetAllEmployerVacanciesAsync(UsernameDto? usernameDto, int pageNumber, string? searchString);
}