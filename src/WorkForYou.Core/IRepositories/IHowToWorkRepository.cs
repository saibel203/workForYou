using WorkForYou.Core.Responses.Repositories;

namespace WorkForYou.Core.IRepositories;

public interface IHowToWorkRepository
{
    Task<HowToWorkResponse> GetAllHowToWorkAsync();
}