using WorkForYou.Core.Models;

namespace WorkForYou.Core.Responses.Repositories;

public class WorkCategoryResponse : BaseResponse
{
    public IReadOnlyList<WorkCategory> WorkCategories { get; set; } = new List<WorkCategory>();
}
