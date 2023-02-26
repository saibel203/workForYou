using WorkForYou.Core.Responses.Repositories;

namespace WorkForYou.Core.RepositoryInterfaces;

public interface IRelocateRepository
{
    Task<RelocateResponse> GetAllRelocatesAsync();
}