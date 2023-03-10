using WorkForYou.Core.Responses.Repositories;

namespace WorkForYou.Core.RepositoryInterfaces;

public interface ICandidateRegionRepository
{
    Task<CandidateRegionResponse> GetAllCandidateRegionsAsync();
}