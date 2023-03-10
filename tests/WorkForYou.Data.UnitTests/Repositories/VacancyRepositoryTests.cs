// using AutoMapper;
// using Microsoft.EntityFrameworkCore;
// using Moq;
// using WorkForYou.Core.Models;
// using WorkForYou.Core.Responses.Repositories;
// using WorkForYou.Data.Repositories;
// using WorkForYou.SharedForTests;
// using Xunit;
//
// namespace WorkForYou.Data.UnitTests.Repositories;
//
// public class VacancyRepositoryTests : TestsBase<VacancyRepository>
// {
//     private readonly VacancyRepository _vacancyRepository;
//     private readonly Mock<IMapper> _mockMapper;
//
//     public VacancyRepositoryTests()
//     {
//         _mockMapper = new Mock<IMapper>();
//         _vacancyRepository = new VacancyRepository(Context, Logger, _mockMapper.Object);
//     }
//
//     [Fact]
//     public async Task GetVacancyByIdAsync_SuccessGetVacancy_ReturnVacancyData()
//     {
//         // Arrange
//         const int vacancyId = 1;
//         const int expectedVacancyCount = 2;
//
//         // Act
//         VacancyResponse getVacancyResponse = await _vacancyRepository.GetVacancyByIdAsync(vacancyId);
//         Vacancy vacancyData = getVacancyResponse.Vacancy;
//         bool vacancyResult = getVacancyResponse.IsSuccessfully;
//
//         int vacancyCount = await Context.Vacancies.CountAsync();
//
//         // Assert
//         Assert.IsAssignableFrom<Vacancy>(vacancyData);
//         Assert.True(vacancyResult);
//         Assert.NotNull(vacancyData);
//         Assert.NotNull(vacancyData.EmployerUser);
//         Assert.Equal(expectedVacancyCount, vacancyCount);
//     }
//     
//     [Fact]
//     public async Task GetVacancyByIdAsync_ErrorGetVacancy_ReturnError()
//     {
//         // Arrange
//         const int vacancyId = 0;
//         const int expectedVacancyCount = 2;
//
//         // Act
//         VacancyResponse getVacancyResponse = await _vacancyRepository.GetVacancyByIdAsync(vacancyId);
//         Vacancy vacancyData = getVacancyResponse.Vacancy;
//         bool vacancyResult = getVacancyResponse.IsSuccessfully;
//
//         int vacancyCount = await Context.Vacancies.CountAsync();
//
//         // Assert
//         Assert.IsAssignableFrom<Vacancy>(vacancyData);
//         Assert.False(vacancyResult);
//         Assert.NotNull(vacancyData);
//         Assert.Null(vacancyData.EmployerUser);
//         Assert.Equal(expectedVacancyCount, vacancyCount);
//     }
// }
