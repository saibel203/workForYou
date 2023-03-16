using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WorkForYou.Core.AdditionalModels;
using WorkForYou.Core.DTOModels.UserDTOs;
using WorkForYou.Core.RepositoryInterfaces;
using WorkForYou.Core.ServiceInterfaces;
using WorkForYou.Shared.ViewModels;

namespace WorkForYou.WebUI.Controllers;

public class FavouriteController : BaseController
{
    private readonly IStringLocalizer<FavouriteController> _stringLocalization;
    private readonly IFavouriteListService _favouriteListService;
    private readonly INotificationService _notificationService;
    private readonly IUnitOfWork _unitOfWork;

    public FavouriteController(IStringLocalizer<FavouriteController> stringLocalization, INotificationService notificationService, IUnitOfWork unitOfWork, IFavouriteListService favouriteListService)
    {
        _stringLocalization = stringLocalization;
        _notificationService = notificationService;
        _unitOfWork = unitOfWork;
        _favouriteListService = favouriteListService;
    }
    
    [HttpGet]
    [Authorize(Roles = ApplicationRoles.EmployerRole)]
    public async Task<IActionResult> FavouriteEmployerList(QueryParameters queryParameters)
    {
        var username = GetUsername();
        var userRole = GetUserRole();
        var usernameDto = new UsernameDto {Username = username, UserRole = userRole};

        var userData = await _unitOfWork.UserRepository
            .GetUserDataAsync(usernameDto);
        var candidates = await _favouriteListService
            .GetFavouriteCandidateListAsync(usernameDto, queryParameters);

        var workCategories = await _unitOfWork.WorkCategoryRepository.GetAllWorkCategoriesAsync();
        var englishLevels = await _unitOfWork.EnglishLevelRepository.GetAllEnglishLevelsAsync();
        var communicationLanguages =
            await _unitOfWork.CommunicationLanguageRepository.GetAllCommunicationLanguagesAsync();

        ViewData["EmployerId"] = userData.User.EmployerUser!.EmployerUserId;

        var candidatesViewModel = new CandidatesViewModel
        {
            QueryParameters = queryParameters,
            CurrentController = ControllerContext.RouteData.Values["controller"]?.ToString(),
            CurrentAction = ControllerContext.RouteData.Values["action"]?.ToString(),
            PageCount = candidates.PageCount,
            VacancyCount = candidates.VacancyCount,
            CandidateUsers = candidates.FavouriteCandidateList,
            Pages = candidates.Pages,
            Username = username,
            WorkCategories = workCategories.WorkCategories,
            EnglishLevels = englishLevels.EnglishLevels,
            CommunicationLanguages = communicationLanguages.CommunicationLanguages
        };

        if (queryParameters.PageNumber < 1)
        {
            if (queryParameters.PageNumber == 0 && !string.IsNullOrEmpty(queryParameters.SearchString)
                || candidatesViewModel.VacancyCount == 0)
            {
                _notificationService.CustomErrorMessage(_stringLocalization["ShowFavouriteListError"]);
                return View(candidatesViewModel);
            }

            queryParameters.PageNumber = 1;
            return RedirectToAction(nameof(FavouriteEmployerList), new
            {
                queryParameters.PageNumber, queryParameters.SearchString, queryParameters.SortBy,
                queryParameters.Username, queryParameters.WorkCategory, queryParameters.EnglishLevel,
                queryParameters.CommunicationLanguages
            });
        }

        if (queryParameters.PageNumber > candidates.PageCount)
        {
            queryParameters.PageNumber = candidates.PageCount;
            return RedirectToAction(nameof(FavouriteEmployerList), new
            {
                queryParameters.PageNumber, queryParameters.SearchString,
                queryParameters.SortBy, queryParameters.Username, queryParameters.WorkCategory,
                queryParameters.EnglishLevel, queryParameters.CommunicationLanguages
            });
        }

        return View(candidatesViewModel);
    }

    [HttpGet]
    [Authorize(Roles = ApplicationRoles.CandidateRole)]
    public async Task<IActionResult> FavouriteCandidateList(QueryParameters queryParameters)
    {
        var username = GetUsername();
        var userRole = GetUserRole();
        var usernameDto = new UsernameDto {Username = username, UserRole = userRole};
        
        var userData = await _unitOfWork.UserRepository
            .GetUserDataAsync(usernameDto);
        
        var favouriteVacancies =
            await _favouriteListService
                .GetFavouriteVacancyListAsync(usernameDto, queryParameters);
        
        var workCategories = await _unitOfWork.WorkCategoryRepository.GetAllWorkCategoriesAsync();
        var englishLevels = await _unitOfWork.EnglishLevelRepository.GetAllEnglishLevelsAsync();
        var typesOfCompany = await _unitOfWork.TypeOfCompanyRepository.GetAllTypesOfCompanyAsync();
        var howToWorks = await _unitOfWork.HowToWorkRepository.GetAllHowToWorkAsync();
        var candidateRegions = await _unitOfWork.CandidateRegionRepository.GetAllCandidateRegionsAsync();


        ViewData["CandidateId"] = userData.User.CandidateUser!.CandidateUserId;

        var vacanciesViewModel = new VacanciesViewModel
        {
            QueryParameters = queryParameters,
            CurrentController = ControllerContext.RouteData.Values["controller"]?.ToString(),
            CurrentAction = ControllerContext.RouteData.Values["action"]?.ToString(),
            PageCount = favouriteVacancies.PageCount,
            VacancyCount = favouriteVacancies.VacancyCount,
            Vacancies = favouriteVacancies.FavouriteVacancyList,
            Pages = favouriteVacancies.Pages,
            Username = username,
            WorkCategories = workCategories.WorkCategories,
            EnglishLevels = englishLevels.EnglishLevels,
            TypesOfCompanies = typesOfCompany.TypesOfCompanies,
            HowToWorks = howToWorks.HowToWorks,
            CandidateRegions = candidateRegions.CandidateRegions
        };
        
        if (queryParameters.PageNumber < 1)
        {
            if (vacanciesViewModel.PageCount == 0)
            {
                _notificationService.CustomErrorMessage(_stringLocalization["ShowFavouriteListError"]);
                return View(vacanciesViewModel);
            }

            queryParameters.PageNumber = 1;
            return RedirectToAction(vacanciesViewModel.CurrentAction, new
            {
                queryParameters.PageNumber, queryParameters.SearchString,
                queryParameters.SortBy, username, queryParameters.WorkCategory, queryParameters.EnglishLevel,
                queryParameters.CandidateRegion, queryParameters.CompanyType, queryParameters.HowToWork
            });
        }

        if (queryParameters.PageNumber > favouriteVacancies.PageCount)
        {
            queryParameters.PageNumber = favouriteVacancies.PageCount;
            return RedirectToAction(vacanciesViewModel.CurrentAction, new
            {
                queryParameters.PageNumber, queryParameters.SearchString,
                queryParameters.SortBy, username, queryParameters.WorkCategory, queryParameters.EnglishLevel,
                queryParameters.CandidateRegion, queryParameters.CompanyType, queryParameters.HowToWork
            });
        }

        return View(vacanciesViewModel);
    }
}
