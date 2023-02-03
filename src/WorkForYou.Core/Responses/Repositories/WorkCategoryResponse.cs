using WorkForYou.Core.Models;

namespace WorkForYou.Core.Responses.Repositories;

public class WorkCategoryResponse : BaseResponse
{
    public IEnumerable<WorkCategory> WorkCategories { get; set; } = new List<WorkCategory>();
}
