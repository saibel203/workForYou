using WorkForYou.Core.Models;

namespace WorkForYou.Core.Responses.Repositories;

public class HowToWorkResponse : BaseResponse
{
    public IEnumerable<HowToWork> HowToWorks { get; set; } = new List<HowToWork>();
}
