using WorkForYou.Core.Responses.Repositories;

namespace WorkForYou.Core.IRepositories;

public interface IVacancyDomainRepository
{
    Task<VacancyDomainResponse> GetAllVacancyDomainsAsync();
}