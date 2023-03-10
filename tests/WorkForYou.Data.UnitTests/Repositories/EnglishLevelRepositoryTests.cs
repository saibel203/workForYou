using WorkForYou.Core.Models;
using WorkForYou.Data.Repositories;
using WorkForYou.SharedForTests;
using Xunit;

namespace WorkForYou.Data.UnitTests.Repositories;

public class EnglishLevelRepositoryTests : TestsBase<EnglishLevelRepository>
{
    private readonly EnglishLevelRepository _englishLevelRepository;

    public EnglishLevelRepositoryTests()
    {
        _englishLevelRepository = new EnglishLevelRepository(Context, Logger);
    }

    [Fact]
    public async Task GetAllEnglishLevelsAsync_EnglishLevelsListNotEmpty_ExpectReturnLevelsList()
    {
        // Arrange
        var testsData = new List<EnglishLevel>
        {
            new()
            {
                EnglishLevelId = 1,
                NameLevel = "Level 1"
            }
        };

        // Act
        var englishLevelsResponse = await _englishLevelRepository.GetAllEnglishLevelsAsync();
        var englishLevelsList = englishLevelsResponse.EnglishLevels;
        var englishLevelsResult = englishLevelsResponse.IsSuccessfully;

        // Assert
        Assert.IsAssignableFrom<IReadOnlyList<EnglishLevel>>(englishLevelsList);
        Assert.Equivalent(testsData, englishLevelsList);
        Assert.Single(englishLevelsList);
        Assert.NotEmpty(englishLevelsList);
        Assert.True(englishLevelsResult);
    }
}
