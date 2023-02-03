using WorkForYou.Core.Models;

namespace WorkForYou.Core.Responses.Repositories;

public class CandidateRegionResponse : BaseResponse
{
    public IEnumerable<CandidateRegion> CandidateRegions { get; set; } = new List<CandidateRegion>();
}
