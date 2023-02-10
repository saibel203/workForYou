using WorkForYou.Core.Responses.Repositories;

namespace WorkForYou.Core.IRepositories;

public interface IRelocateRepository
{
    Task<RelocateResponse> GetAllRelocatesAsync();
}