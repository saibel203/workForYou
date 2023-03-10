using WorkForYou.Core.Models;
using WorkForYou.Data.Repositories;
using WorkForYou.SharedForTests;
using Xunit;

namespace WorkForYou.Data.UnitTests.Repositories;

public class RelocateRepositoryTests : TestsBase<RelocateRepository>
{
    private readonly RelocateRepository _relocateRepository;

    public RelocateRepositoryTests()
    {
        _relocateRepository = new RelocateRepository(Context, Logger);
    }
    
    [Fact]
    public async Task GetAllRelocatesAsync_RelocateListNotEmpty_ExpectReturnRelocateList()
    {
        var testsData = new List<Relocate>
        {
            new()
            {
                RelocateId = 1,
                RelocateName = "Relocate 1"
            }
        };

        // Act
        var relocateResponse = await _relocateRepository.GetAllRelocatesAsync();
        var relocateList = relocateResponse.Relocates;
        var relocateResult = relocateResponse.IsSuccessfully;

        // Assert
        Assert.IsAssignableFrom<IReadOnlyList<Relocate>>(relocateList);
        Assert.Equivalent(testsData, relocateList);
        Assert.Single(relocateList);
        Assert.NotEmpty(relocateList);
        Assert.True(relocateResult);
    }
}
