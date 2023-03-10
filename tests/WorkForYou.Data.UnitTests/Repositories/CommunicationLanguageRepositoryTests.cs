using WorkForYou.Core.Models;
using WorkForYou.Data.Repositories;
using WorkForYou.SharedForTests;
using Xunit;

namespace WorkForYou.Data.UnitTests.Repositories;

public class CommunicationLanguageRepositoryTests : TestsBase<CommunicationLanguageRepository>
{
    private readonly CommunicationLanguageRepository _communicationLanguageRepository;

    public CommunicationLanguageRepositoryTests()
    {
        _communicationLanguageRepository = new CommunicationLanguageRepository(Context, Logger);
    }

    [Fact]
    public async Task GetAllCommunicationLanguagesAsync_LanguagesListNotEmpty_ExpectReturnLanguagesList()
    {
        // Arrange
        var testsData = new List<CommunicationLanguage>
        {
            new()
            {
                CommunicationLanguageId = 1,
                CommunicationLanguageName = "Language 1"
            }
        };

        // Act
        var communicationLanguageResponse = await _communicationLanguageRepository.GetAllCommunicationLanguagesAsync();
        var communicationLanguageList = communicationLanguageResponse.CommunicationLanguages;
        var communicationLanguageResult = communicationLanguageResponse.IsSuccessfully;

        // Assert
        Assert.IsAssignableFrom<IReadOnlyList<CommunicationLanguage>>(communicationLanguageList);
        Assert.Equivalent(testsData, communicationLanguageList);
        Assert.Single(communicationLanguageList);
        Assert.NotEmpty(communicationLanguageList);
        Assert.True(communicationLanguageResult);
    }
}
