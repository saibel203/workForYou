using WorkForYou.Core.Responses.Repositories;

namespace WorkForYou.Core.IRepositories;

public interface ICommunicationLanguageRepository
{
    Task<CommunicationLanguageResponse> GetAllCommunicationLanguagesAsync();
}