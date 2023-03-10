using WorkForYou.Core.Models;
using WorkForYou.Data.Repositories;
using WorkForYou.SharedForTests;
using Xunit;

namespace WorkForYou.Data.UnitTests.Repositories;

public class CandidateRegionRepositoryTests : TestsBase<CandidateRegionRepository>
{
    private readonly CandidateRegionRepository _candidateRegionRepository;

    public CandidateRegionRepositoryTests()
    {
        _candidateRegionRepository = new CandidateRegionRepository(Context, Logger);
    }
    
    [Fact]
    public async Task GetAllCandidateRegionsAsync_RegionListNotEmpty_ExpectReturnRegionsList()
    {
        // Arrange
        var testsData = new List<CandidateRegion>
        {
            new() {CandidateRegionId = 1, CandidateRegionName = "Name 1"}
        };
        
        // Act
        var candidateRegionResponse = await _candidateRegionRepository.GetAllCandidateRegionsAsync();
        var candidateRegionsList = candidateRegionResponse.CandidateRegions;
        var candidateRegionResult = candidateRegionResponse.IsSuccessfully;

        // Assert
        Assert.IsAssignableFrom<IReadOnlyList<CandidateRegion>>(candidateRegionsList);
        Assert.Equivalent(testsData, candidateRegionsList);
        Assert.Single(candidateRegionsList);
        Assert.NotEmpty(candidateRegionsList);
        Assert.True(candidateRegionResult);
    }
}