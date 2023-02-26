using WorkForYou.Core.Responses.Repositories;

namespace WorkForYou.Core.RepositoryInterfaces;

public interface ICommunicationLanguageRepository
{
    Task<CommunicationLanguageResponse> GetAllCommunicationLanguagesAsync();
}