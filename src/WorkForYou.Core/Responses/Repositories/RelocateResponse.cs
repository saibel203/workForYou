using WorkForYou.Core.Models;

namespace WorkForYou.Core.Responses.Repositories;

public class RelocateResponse : BaseResponse
{
    public IReadOnlyList<Relocate>? Relocates { get; set; } = new List<Relocate>();
}
