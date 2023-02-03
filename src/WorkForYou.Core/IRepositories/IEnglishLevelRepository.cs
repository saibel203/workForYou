using WorkForYou.Core.Responses.Repositories;

namespace WorkForYou.Core.IRepositories;

public interface IEnglishLevelRepository
{
    Task<EnglishLevelResponse> GetAllEnglishLevelsAsync();
}