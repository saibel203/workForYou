using Microsoft.EntityFrameworkCore;
using WorkForYou.Core.AdditionalModels;
using WorkForYou.Core.DTOModels.UserDTOs;
using WorkForYou.Core.Models.IdentityInheritance;
using WorkForYou.Core.Responses.Repositories;
using WorkForYou.Data.Repositories;
using WorkForYou.SharedForTests;
using Xunit;

namespace WorkForYou.Data.UnitTests.Repositories;

public class UserRepositoryTests : TestsBase<UserRepository>
{
    private readonly UserRepository _userRepository;

    public UserRepositoryTests()
    {
        _userRepository = new UserRepository(Context, Logger);
    }

    [Fact]
    public async Task GetUserDataAsync_SuccessGetEmployerUser_ReturnEmployerUserData()
    {
        // Arrange
        UsernameDto testsData = new UsernameDto
        {
            Username = "username 1", UserRole = "employer"
        };

        // Act
        UserResponse userResponse = await _userRepository.GetUserDataAsync(testsData);
        ApplicationUser userData = userResponse.User;
        bool userResult = userResponse.IsSuccessfully;

        // Assert
        Assert.IsAssignableFrom<ApplicationUser>(userData);
        Assert.NotNull(userData);
        Assert.NotNull(userData.EmployerUser);
        Assert.Null(userData.CandidateUser);
        Assert.True(userResult);
    }

    [Fact]
    public async Task GetUserDataAsync_SuccessGetCandidateUser_ReturnCandidateUserData()
    {
        // Arrange
        UsernameDto testsData = new UsernameDto
        {
            Username = "username 2", UserRole = "candidate"
        };

        // Act
        UserResponse userResponse = await _userRepository.GetUserDataAsync(testsData);
        ApplicationUser userData = userResponse.User;
        bool userResult = userResponse.IsSuccessfully;

        // Assert
        Assert.IsAssignableFrom<ApplicationUser>(userData);
        Assert.NotNull(userData);
        Assert.NotNull(userData.CandidateUser);
        Assert.Null(userData.EmployerUser);
        Assert.True(userResult);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData(null, "")]
    [InlineData("", null)]
    [InlineData(null, null)]
    [InlineData("username 1", null)]
    [InlineData("username 2", null)]
    public async Task GetUserDataAsync_ErrorGetUserByUsernameOrByRole_ReturnErrorMessage(
        string username, string userRole)
    {
        // Arrange
        UsernameDto testsData = new UsernameDto
        {
            Username = username, UserRole = userRole
        };
        const int expectedUsersCount = 2;

        // Act
        UserResponse userResponse = await _userRepository.GetUserDataAsync(testsData);
        ApplicationUser userData = userResponse.User;
        bool userResult = userResponse.IsSuccessfully;
        int usersCount = Context.Users.Count();

        // Assert
        Assert.IsAssignableFrom<ApplicationUser>(userData);
        Assert.Null(userData.CandidateUser);
        Assert.Null(userData.EmployerUser);
        Assert.False(userResult);
        Assert.Equal(expectedUsersCount, usersCount);
    }

    [Theory]
    [InlineData("username 1", ApplicationRoles.CandidateRole)]
    [InlineData("username 2", ApplicationRoles.EmployerRole)]
    public async Task RemoveUserAsync_SuccessRemoveUser_ReturnSuccessRemoveMessage(string username, string userRole)
    {
        // Arrange
        UsernameDto testsData = new UsernameDto
        {
            Username = username, UserRole = userRole
        };
        const int expectedUsersCount = 1;

        // Act
        UserResponse removeUserResponse = await _userRepository.RemoveUserAsync(testsData);
        bool removeUserResult = removeUserResponse.IsSuccessfully;
        int usersCount = Context.Users.Count();

        // Assert
        Assert.True(removeUserResult);
        Assert.Equal(expectedUsersCount, usersCount);
    }

    [Theory]
    [InlineData("username 1", "")]
    [InlineData("", "")]
    [InlineData(null, "")]
    [InlineData("", null)]
    [InlineData(null, null)]
    public async Task RemoveUserAsync_ErrorRemoveUser_ReturnErrorMessage(string username, string userRole)
    {
        // Arrange
        UsernameDto testsData = new UsernameDto
        {
            Username = username, UserRole = userRole
        };
        const int expectedUsersCount = 2;

        // Act
        UserResponse removeUserResponse = await _userRepository.RemoveUserAsync(testsData);
        bool removeUserResult = removeUserResponse.IsSuccessfully;
        int usersCount = Context.Users.Count();

        // Assert
        Assert.False(removeUserResult);
        Assert.Equal(expectedUsersCount, usersCount);
    }

    [Theory]
    [InlineData("username 1", "candidate")]
    [InlineData("username 2", "employer")]
    public async Task RefreshGeneralInfoAsync_SuccessRefreshGeneralUserInfo_ReturnSuccessMessage(
        string username, string userRole)
    {
        // Arrange
        RefreshGeneralUserDto testsDataForUpdate = new RefreshGeneralUserDto
        {
            UserName = username, UserRole = userRole,
            FirstName = "first name", LastName = "last name"
        };

        UsernameDto testsDataForGetUser = new UsernameDto
        {
            Username = testsDataForUpdate.UserName, UserRole = testsDataForUpdate.UserRole
        };
        const int expectedUsersCount = 2;

        // Act
        UserResponse updateUserResponse = await _userRepository.RefreshGeneralInfoAsync(testsDataForUpdate);
        bool updateUserResult = updateUserResponse.IsSuccessfully;

        UserResponse userDataResponse = await _userRepository.GetUserDataAsync(testsDataForGetUser);
        ApplicationUser userData = userDataResponse.User;
        bool userDataResult = userDataResponse.IsSuccessfully;

        int usersCount = await Context.Users.CountAsync();

        // Assert
        Assert.IsAssignableFrom<ApplicationUser>(userData);
        Assert.True(updateUserResult);
        Assert.True(userDataResult);
        Assert.Equal(userData.FirstName, testsDataForUpdate.FirstName);
        Assert.Equal(userData.LastName, testsDataForUpdate.LastName);
        Assert.Equal(expectedUsersCount, usersCount);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("", "candidate")]
    [InlineData("", "employer")]
    [InlineData("username 1", "")]
    public async Task RefreshGeneralInfoAsync_ErrorRefreshGeneralUserInfo_ReturnErrorMessage(
        string username, string userRole)
    {
        // Arrange
        RefreshGeneralUserDto testsDataForUpdate = new RefreshGeneralUserDto
        {
            UserName = username, UserRole = userRole,
            FirstName = "first name", LastName = "last name"
        };

        UsernameDto testsDataForGetUser = new UsernameDto
        {
            Username = testsDataForUpdate.UserName, UserRole = testsDataForUpdate.UserRole
        };
        const int expectedUsersCount = 2;

        // Act
        UserResponse updateUserResponse = await _userRepository.RefreshGeneralInfoAsync(testsDataForUpdate);
        bool updateUserResult = updateUserResponse.IsSuccessfully;

        UserResponse userDataResponse = await _userRepository.GetUserDataAsync(testsDataForGetUser);
        ApplicationUser userData = userDataResponse.User;
        bool userDataResult = userDataResponse.IsSuccessfully;

        int usersCount = await Context.Users.CountAsync();

        // Assert
        Assert.IsAssignableFrom<ApplicationUser>(userData);
        Assert.False(updateUserResult);
        Assert.False(userDataResult);
        Assert.NotEqual(userData.FirstName, testsDataForUpdate.FirstName);
        Assert.NotEqual(userData.LastName, testsDataForUpdate.LastName);
        Assert.Equal(expectedUsersCount, usersCount);
    }
    
    [Fact]
    public async Task RefreshEmployerInfoAsync_SuccessRefreshEmployerInfo_ReturnSuccessMessageAndResult()
    {
        // Arrange
        RefreshEmployerDto testsDataForRefresh = new RefreshEmployerDto
        {
            Username = "username 1",
            CompanyPosition = "position"
        };

        UsernameDto testsDataForGetUser = new UsernameDto
        {
            Username = "username 1", UserRole = EmployerRole
        };

        const int expectedUsersCount = 2; 

        // Act
        UserResponse refreshEmployerResponse = await _userRepository
            .RefreshEmployerInfoAsync(testsDataForRefresh);
        bool refreshEmployerResult = refreshEmployerResponse.IsSuccessfully;

        UserResponse userDataResponse = await _userRepository.GetUserDataAsync(testsDataForGetUser);
        ApplicationUser userData = userDataResponse.User;
        bool userDataResult = userDataResponse.IsSuccessfully;

        int usersCount = await Context.Users.CountAsync();

        // Assert
        Assert.IsAssignableFrom<ApplicationUser>(userData);
        Assert.True(refreshEmployerResult);
        Assert.True(userDataResult);
        Assert.Null(userData.CandidateUser);
        Assert.NotNull(userData.EmployerUser);
        Assert.Equal(userData.EmployerUser.CompanyPosition, testsDataForRefresh.CompanyPosition);
        Assert.Equal(expectedUsersCount, usersCount);
    }

    [Fact]
    public async Task RefreshCandidateUserInfoAsync_SuccessRefreshCandidateInfo_ReturnSuccessMessageAndResult()
    {
        // Arrange
        RefreshCandidateDto testsDataForRefresh = new RefreshCandidateDto
        {
            Username = "username 2",
            CompanyPosition = "position", City = "city"
        };

        UsernameDto testsDataForGetUser = new UsernameDto
        {
            Username = "username 2", UserRole = CandidateRole
        };

        const int expectedUsersCount = 2; 

        // Act
        UserResponse refreshCandidateResponse = await _userRepository
            .RefreshCandidateUserInfoAsync(testsDataForRefresh);
        bool refreshCandidateResult = refreshCandidateResponse.IsSuccessfully;

        UserResponse userDataResponse = await _userRepository.GetUserDataAsync(testsDataForGetUser);
        ApplicationUser userData = userDataResponse.User;
        bool userDataResult = userDataResponse.IsSuccessfully;

        int usersCount = await Context.Users.CountAsync();

        // Assert
        Assert.IsAssignableFrom<ApplicationUser>(userData);
        Assert.True(refreshCandidateResult);
        Assert.True(userDataResult);
        Assert.NotNull(userData.CandidateUser);
        Assert.Null(userData.EmployerUser);
        Assert.Equal(userData.CandidateUser.CompanyPosition, testsDataForRefresh.CompanyPosition);
        Assert.Equal(userData.CandidateUser.City, testsDataForRefresh.City);
        Assert.Equal(expectedUsersCount, usersCount);
        Assert.False(userData.CandidateUser.IsProfileComplete);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("", "employer")]
    [InlineData("username 2", "")]
    public async Task RefreshEmployerInfoAsync_ErrorRefreshEmployerInfo_ReturnErrorMessage(
        string username, string userRole)
    {
        // Arrange
        RefreshEmployerDto testsDataForRefresh = new RefreshEmployerDto
        {
            Username = username, CompanyPosition = "position"
        };

        UsernameDto testsDataForGetUser = new UsernameDto
        {
            Username = username, UserRole = userRole
        };

        const int expectedUsersCount = 2; 

        // Act
        UserResponse refreshEmployerResponse = await _userRepository
            .RefreshEmployerInfoAsync(testsDataForRefresh);
        bool refreshEmployerResult = refreshEmployerResponse.IsSuccessfully;

        UserResponse userDataResponse = await _userRepository.GetUserDataAsync(testsDataForGetUser);
        ApplicationUser userData = userDataResponse.User;
        bool userDataResult = userDataResponse.IsSuccessfully;

        int usersCount = await Context.Users.CountAsync();

        // Assert
        Assert.IsAssignableFrom<ApplicationUser>(userData);
        Assert.False(refreshEmployerResult);
        Assert.False(userDataResult);
        Assert.Null(userData.CandidateUser);
        Assert.Null(userData.EmployerUser);
        Assert.Equal(expectedUsersCount, usersCount);
    }
    
    [Theory]
    [InlineData("", "")]
    [InlineData("", "candidate")]
    [InlineData("username 1", "")]
    public async Task RefreshCandidateInfoAsync_ErrorRefreshCandidateInfo_ReturnErrorMessage(
        string username, string userRole)
    {
        // Arrange
        RefreshCandidateDto testsDataForRefresh = new RefreshCandidateDto
        {
            Username = username,
            CompanyPosition = "position", City = "city"
        };

        UsernameDto testsDataForGetUser = new UsernameDto
        {
            Username = username, UserRole = userRole
        };

        const int expectedUsersCount = 2; 

        // Act
        UserResponse refreshCandidateResponse = await _userRepository
            .RefreshCandidateUserInfoAsync(testsDataForRefresh);
        bool refreshCandidateResult = refreshCandidateResponse.IsSuccessfully;

        UserResponse userDataResponse = await _userRepository.GetUserDataAsync(testsDataForGetUser);
        ApplicationUser userData = userDataResponse.User;
        bool userDataResult = userDataResponse.IsSuccessfully;

        int usersCount = await Context.Users.CountAsync();

        // Assert
        Assert.IsAssignableFrom<ApplicationUser>(userData);
        Assert.False(refreshCandidateResult);
        Assert.False(userDataResult);
        Assert.Null(userData.CandidateUser);
        Assert.Null(userData.EmployerUser);
        Assert.Equal(expectedUsersCount, usersCount);
    }

    [Fact]
    public async Task RefreshCandidateInfoAsync_SuccessRefreshAndCheckProfileCompleteTrue_ReturnErrorMessage()
    {
        // Arrange
        RefreshCandidateDto testsDataForRefresh = new RefreshCandidateDto
        {
            Username = "username 2",
            CompanyPosition = "position", City = "city", ExpectedSalary = 1,
            ExperienceWorkTime = 1, ExperienceWorkDescription = "description",
            EnglishLevelId = 1
        };

        UsernameDto testsDataForGetUser = new UsernameDto
        {
            Username = "username 2", UserRole = CandidateRole
        };

        const int expectedUsersCount = 2; 

        // Act
        UserResponse refreshCandidateResponse = await _userRepository
            .RefreshCandidateUserInfoAsync(testsDataForRefresh);
        bool refreshCandidateResult = refreshCandidateResponse.IsSuccessfully;

        UserResponse userDataResponse = await _userRepository.GetUserDataAsync(testsDataForGetUser);
        ApplicationUser userData = userDataResponse.User;
        bool userDataResult = userDataResponse.IsSuccessfully;

        int usersCount = await Context.Users.CountAsync();

        // Assert
        Assert.IsAssignableFrom<ApplicationUser>(userData);
        Assert.True(refreshCandidateResult);
        Assert.True(userDataResult);
        Assert.NotNull(userData.CandidateUser);
        Assert.Null(userData.EmployerUser);
        Assert.Equal(userData.CandidateUser.CompanyPosition, testsDataForRefresh.CompanyPosition);
        Assert.Equal(userData.CandidateUser.City, testsDataForRefresh.City);
        Assert.Equal(userData.CandidateUser.EnglishLevelId, testsDataForRefresh.EnglishLevelId);
        Assert.Equal(userData.CandidateUser.ExperienceWorkDescription, testsDataForRefresh.ExperienceWorkDescription);
        Assert.Equal(userData.CandidateUser.ExperienceWorkTime, testsDataForRefresh.ExperienceWorkTime);
        Assert.Equal(userData.CandidateUser.ExpectedSalary, testsDataForRefresh.ExpectedSalary);
        Assert.Equal(expectedUsersCount, usersCount);
        Assert.True(userData.CandidateUser.IsProfileComplete);
    }

    [Theory]
    [InlineData("", 0, 0, "", null)]
    [InlineData("position", 0, 0, "description", null)]
    [InlineData("position", 0, 0, "", null)]
    [InlineData("", 1, 0, "", null)]
    [InlineData("", 1, 1, "", null)]
    [InlineData("", 0, 0, "", 1)]
    [InlineData("position", 1, 1, "description", null)]
    [InlineData("", 1, 1, "description", 1)]
    public async Task RefreshCandidateInfoAsync_SuccessRefreshAndCheckProfileCompleteFalse_ReturnErrorMessage(
        string companyPosition, int salary, int experience, string experienceDescription, int? englishLevelId)
    {
        // Arrange
        RefreshCandidateDto testsDataForRefresh = new RefreshCandidateDto
        {
            Username = "username 2",
            CompanyPosition = companyPosition, ExpectedSalary = salary,
            ExperienceWorkTime = experience, ExperienceWorkDescription = experienceDescription,
            EnglishLevelId = englishLevelId
        };

        UsernameDto testsDataForGetUser = new UsernameDto
        {
            Username = "username 2", UserRole = CandidateRole
        };

        const int expectedUsersCount = 2; 

        // Act
        UserResponse refreshCandidateResponse = await _userRepository
            .RefreshCandidateUserInfoAsync(testsDataForRefresh);
        bool refreshCandidateResult = refreshCandidateResponse.IsSuccessfully;

        UserResponse userDataResponse = await _userRepository.GetUserDataAsync(testsDataForGetUser);
        ApplicationUser userData = userDataResponse.User;
        bool userDataResult = userDataResponse.IsSuccessfully;

        int usersCount = await Context.Users.CountAsync();

        // Assert
        Assert.IsAssignableFrom<ApplicationUser>(userData);
        Assert.True(refreshCandidateResult);
        Assert.True(userDataResult);
        Assert.NotNull(userData.CandidateUser);
        Assert.Null(userData.EmployerUser);
        Assert.Equal(expectedUsersCount, usersCount);
        Assert.False(userData.CandidateUser.IsProfileComplete);
    }
}