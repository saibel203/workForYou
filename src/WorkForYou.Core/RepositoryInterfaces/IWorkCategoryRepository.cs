using WorkForYou.Core.Responses.Repositories;

namespace WorkForYou.Core.RepositoryInterfaces;

public interface IWorkCategoryRepository
{
    Task<WorkCategoryResponse> GetAllWorkCategoriesAsync();
}