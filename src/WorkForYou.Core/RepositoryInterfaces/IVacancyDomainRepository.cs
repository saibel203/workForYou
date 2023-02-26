using WorkForYou.Core.Responses.Repositories;

namespace WorkForYou.Core.RepositoryInterfaces;

public interface IVacancyDomainRepository
{
    Task<VacancyDomainResponse> GetAllVacancyDomainsAsync();
}