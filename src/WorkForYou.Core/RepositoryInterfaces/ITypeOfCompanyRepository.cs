using WorkForYou.Core.Responses.Repositories;

namespace WorkForYou.Core.RepositoryInterfaces;

public interface ITypeOfCompanyRepository
{
    Task<TypesOfCompanyResponse> GetAllTypesOfCompanyAsync();
}