using WorkForYou.Core.Responses.Repositories;

namespace WorkForYou.Core.RepositoryInterfaces;

public interface IHowToWorkRepository
{
    Task<HowToWorkResponse> GetAllHowToWorkAsync();
}