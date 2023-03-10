using WorkForYou.Core.Models;
using WorkForYou.Data.Repositories;
using WorkForYou.SharedForTests;
using Xunit;

namespace WorkForYou.Data.UnitTests.Repositories;

public class TypeOfCompanyRepositoryTests : TestsBase<TypeOfCompanyRepository>
{
    private readonly TypeOfCompanyRepository _typeOfCompanyRepository;

    public TypeOfCompanyRepositoryTests()
    {
        _typeOfCompanyRepository = new TypeOfCompanyRepository(Context, Logger);
    }
    
    [Fact]
    public async Task GetAllTypesOfCompanyAsync_TypeOfCompanyListNotEmpty_ExpectReturnTypeOfCompanyList()
    {
        // Arrange
        var testsData = new List<TypesOfCompany>
        {
            new()
            {
                TypesOfCompanyId = 1,
                TypeOfCompanyName = "TypeOfCompany 1"
            }
        };

        // Act
        var typeOfCompanyResponse = await _typeOfCompanyRepository.GetAllTypesOfCompanyAsync();
        var typeOfCompanyList = typeOfCompanyResponse.TypesOfCompanies;
        var typeOfCompanyResult = typeOfCompanyResponse.IsSuccessfully;

        // Assert
        Assert.IsAssignableFrom<IReadOnlyList<TypesOfCompany>>(typeOfCompanyList);
        Assert.Equivalent(testsData, typeOfCompanyList);
        Assert.Single(typeOfCompanyList);
        Assert.NotEmpty(typeOfCompanyList);
        Assert.True(typeOfCompanyResult);
    }
}
