using WorkForYou.Core.Models;
using WorkForYou.Data.Repositories;
using WorkForYou.SharedForTests;
using Xunit;

namespace WorkForYou.Data.UnitTests.Repositories;

public class HowToWorkRepositoryTests : TestsBase<HowToWorkRepository>
{
    private readonly HowToWorkRepository _howToWorkRepository;

    public HowToWorkRepositoryTests()
    {
        _howToWorkRepository = new HowToWorkRepository(Context, Logger);
    }

    [Fact]
    public async Task GetAllHowToWorkAsync_HowToWorkListNotEmpty_ExpectReturnHowToWorkList()
    {
        // Arrange
        var testsData = new List<HowToWork>
        {
            new()
            {
                HowToWorkId = 1, 
                HowToWorkName = "HowToWork 1"
            }
        };

        // Act
        var howToWorkResponse = await _howToWorkRepository.GetAllHowToWorkAsync();
        var howToWorkList = howToWorkResponse.HowToWorks;
        var howToWorkResult = howToWorkResponse.IsSuccessfully;

        // Assert
        Assert.IsAssignableFrom<IReadOnlyList<HowToWork>>(howToWorkList);
        Assert.Equivalent(testsData, howToWorkList);
        Assert.Single(howToWorkList);
        Assert.NotEmpty(howToWorkList);
        Assert.True(howToWorkResult);
    }
}