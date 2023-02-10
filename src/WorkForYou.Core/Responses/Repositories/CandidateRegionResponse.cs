using WorkForYou.Core.Models;

namespace WorkForYou.Core.Responses.Repositories;

public class CandidateRegionResponse : BaseResponse
{
    public IReadOnlyList<CandidateRegion> CandidateRegions { get; set; } = new List<CandidateRegion>();
}
