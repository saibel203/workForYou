using WorkForYou.Core.Responses.Repositories;

namespace WorkForYou.Core.IRepositories;

public interface IWorkCategoryRepository
{
    Task<WorkCategoryResponse> GetAllWorkCategoriesAsync();
}