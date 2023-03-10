using WorkForYou.Core.Models;
using WorkForYou.Data.Repositories;
using WorkForYou.SharedForTests;
using Xunit;

namespace WorkForYou.Data.UnitTests.Repositories;

public class WorkCategoryRepositoryTests : TestsBase<WorkCategoryRepository>
{
    private readonly WorkCategoryRepository _workCategoryRepository;

    public WorkCategoryRepositoryTests()
    {
        _workCategoryRepository = new WorkCategoryRepository(Context, Logger);
    }
    
    [Fact]
    public async Task GetAllWorkCategoriesAsync_WorkCategoriesListNotEmpty_ExpectReturnCategoriesList()
    {
        // Arrange
        var testsData = new List<WorkCategory>
        {
            new()
            {
                WorkCategoryId = 1,
                CategoryName = "WorkCategory 1"
            }
        };

        // Act
        var workCategoryResponse = await _workCategoryRepository.GetAllWorkCategoriesAsync();
        var workCategoryList = workCategoryResponse.WorkCategories;
        var workCategoryResult = workCategoryResponse.IsSuccessfully;

        // Assert
        Assert.IsAssignableFrom<IReadOnlyList<WorkCategory>>(workCategoryList);
        Assert.Equivalent(testsData, workCategoryList);
        Assert.Single(workCategoryList);
        Assert.NotEmpty(workCategoryList);
        Assert.True(workCategoryResult);
    }
}
