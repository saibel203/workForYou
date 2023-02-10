using WorkForYou.Core.Models;

namespace WorkForYou.Core.Responses.Repositories;

public class HowToWorkResponse : BaseResponse
{
    public IReadOnlyList<HowToWork> HowToWorks { get; set; } = new List<HowToWork>();
}
