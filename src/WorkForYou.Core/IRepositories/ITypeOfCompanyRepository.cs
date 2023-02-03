using WorkForYou.Core.Responses.Repositories;

namespace WorkForYou.Core.IRepositories;

public interface ITypeOfCompanyRepository
{
    Task<TypesOfCompanyResponse> GetAllTypesOfCompanyAsync();
}