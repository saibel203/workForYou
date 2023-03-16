using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WorkForYou.Core.AdditionalModels;
using WorkForYou.Core.DTOModels.UserDTOs;
using WorkForYou.Core.RepositoryInterfaces;
using WorkForYou.Core.ServiceInterfaces;
using WorkForYou.Shared.ViewModels;

namespace WorkForYou.WebUI.Controllers;

public class RespondedController : BaseController
{
    private readonly IStringLocalizer<RespondedController> _stringLocalization;
    private readonly INotificationService _notificationService;
    private readonly IUnitOfWork _unitOfWork;

    public RespondedController(INotificationService notificationService, IUnitOfWork unitOfWork,
        IStringLocalizer<RespondedController> stringLocalization)
    {
        _notificationService = notificationService;
        _unitOfWork = unitOfWork;
        _stringLocalization = stringLocalization;
    }

    [HttpGet]
    [Authorize(Roles = ApplicationRoles.EmployerRole)]
    public async Task<IActionResult> AllVacancyResponses(QueryParameters queryParameters, int vacancyId)
    {
        var username = GetUsername();
        var usernameDto = new UsernameDto {Username = username, UserRole = ApplicationRoles.EmployerRole};
        var userData = await _unitOfWork.UserRepository
            .GetUserDataAsync(usernameDto);

        var workCategories = await _unitOfWork.WorkCategoryRepository.GetAllWorkCategoriesAsync();
        var englishLevels = await _unitOfWork.EnglishLevelRepository.GetAllEnglishLevelsAsync();
        var communicationLanguages =
            await _unitOfWork.CommunicationLanguageRepository.GetAllCommunicationLanguagesAsync();

        ViewData["EmployerId"] = userData.User.EmployerUser!.EmployerUserId;

        var vacancyResponsesResult = await _unitOfWork.RespondedListRepository
            .AllVacancyResponses(vacancyId, queryParameters);

        var candidatesViewModel = new CandidatesViewModel
        {
            QueryParameters = queryParameters,
            CurrentController = ControllerContext.RouteData.Values["controller"]?.ToString(),
            CurrentAction = ControllerContext.RouteData.Values["action"]?.ToString(),
            PageCount = vacancyResponsesResult.PageCount,
            VacancyCount = vacancyResponsesResult.VacancyCount,
            ApplicationUsers = vacancyResponsesResult.ApplicationUsers,
            Pages = vacancyResponsesResult.Pages,
            Username = username,
            CurrentVacancyId = vacancyId,
            WorkCategories = workCategories.WorkCategories,
            EnglishLevels = englishLevels.EnglishLevels,
            CommunicationLanguages = communicationLanguages.CommunicationLanguages
        };

        if (queryParameters.PageNumber < 1)
        {
            if (queryParameters.PageNumber == 0 && !string.IsNullOrEmpty(queryParameters.SearchString)
                || candidatesViewModel.VacancyCount == 0)
            {
                _notificationService.CustomErrorMessage(_stringLocalization["VacanciesNotFound"]);
                return View(candidatesViewModel);
            }

            queryParameters.PageNumber = 1;
            return RedirectToAction(nameof(AllVacancyResponses), new
            {
                queryParameters.PageNumber, queryParameters.SearchString, queryParameters.SortBy,
                queryParameters.Username, vacancyId, queryParameters.WorkCategory, queryParameters.EnglishLevel,
                queryParameters.CommunicationLanguages
            });
        }

        if (queryParameters.PageNumber > vacancyResponsesResult.PageCount)
        {
            queryParameters.PageNumber = vacancyResponsesResult.PageCount;
            return RedirectToAction(nameof(AllVacancyResponses), new
            {
                queryParameters.PageNumber, queryParameters.SearchString,
                queryParameters.SortBy, queryParameters.Username, vacancyId,
                queryParameters.WorkCategory, queryParameters.EnglishLevel,
                queryParameters.CommunicationLanguages
            });
        }

        return View(candidatesViewModel);
    }
    
    [HttpGet]
    [Authorize(Roles = ApplicationRoles.CandidateRole)]
    public async Task<IActionResult> RespondedList(QueryParameters queryParameters)
    {
        var username = GetUsername();
        var usernameDto = new UsernameDto {Username = username, UserRole = ApplicationRoles.CandidateRole};

        var userData = await _unitOfWork.UserRepository.GetUserDataAsync(usernameDto);
        var respondedList = await _unitOfWork.RespondedListRepository
            .AllCandidateRespondedAsync(usernameDto, queryParameters);
        
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
            PageCount = respondedList.PageCount,
            VacancyCount = respondedList.VacancyCount,
            Vacancies = respondedList.VacancyList,
            Pages = respondedList.Pages,
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
                _notificationService.CustomErrorMessage(_stringLocalization["VacanciesNotFound"]);
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

        if (queryParameters.PageNumber > respondedList.PageCount)
        {
            queryParameters.PageNumber = respondedList.PageCount;
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