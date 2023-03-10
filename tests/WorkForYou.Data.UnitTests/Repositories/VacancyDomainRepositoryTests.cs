using WorkForYou.Core.Models;
using WorkForYou.Data.Repositories;
using WorkForYou.SharedForTests;
using Xunit;

namespace WorkForYou.Data.UnitTests.Repositories;

public class VacancyDomainRepositoryTests : TestsBase<VacancyDomainRepository>
{
    private readonly VacancyDomainRepository _vacancyDomainRepository;

    public VacancyDomainRepositoryTests()
    {
        _vacancyDomainRepository = new VacancyDomainRepository(Context, Logger);
    }
    
    [Fact]
    public async Task GetAllVacancyDomainsAsync_VacancyDomainNotEmpty_ExpectReturnVacancyDomainList()
    {
        // Arrange
        var testsData = new List<VacancyDomain>
        {
            new()
            {
                VacancyDomainId = 1,
                VacancyDomainName = "VacancyDomain 1"
            }
        };

        // Act
        var vacancyDomainResponse = await _vacancyDomainRepository.GetAllVacancyDomainsAsync();
        var vacancyDomainList = vacancyDomainResponse.VacancyDomains;
        var vacancyDomainResult = vacancyDomainResponse.IsSuccessfully;

        // Assert
        Assert.IsAssignableFrom<IReadOnlyList<VacancyDomain>>(vacancyDomainList);
        Assert.Equivalent(testsData, vacancyDomainList);
        Assert.Single(vacancyDomainList);
        Assert.NotEmpty(vacancyDomainList);
        Assert.True(vacancyDomainResult);
    }
}
