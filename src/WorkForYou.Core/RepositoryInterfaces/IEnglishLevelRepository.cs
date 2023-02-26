using WorkForYou.Core.Responses.Repositories;

namespace WorkForYou.Core.RepositoryInterfaces;

public interface IEnglishLevelRepository
{
    Task<EnglishLevelResponse> GetAllEnglishLevelsAsync();
}