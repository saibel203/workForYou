using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WorkForYou.Core.AdditionalModels;
using WorkForYou.Core.DTOModels.UserDTOs;
using WorkForYou.Core.RepositoryInterfaces;
using WorkForYou.Core.Responses;
using WorkForYou.Core.ServiceInterfaces;
using WorkForYou.Shared.ViewModels;
using WorkForYou.Shared.ViewModels.AdditionalViewModels;
using WorkForYou.Shared.ViewModels.Forms;

namespace WorkForYou.WebUI.Controllers;

[Authorize(Roles = ApplicationRoles.CandidateRole)]
public class CandidateAccountController : BaseController
{
    private readonly IStringLocalizer<CandidateAccountController> _stringLocalization;
    private readonly INotificationService _notificationService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CandidateAccountController(IUnitOfWork unitOfWork, INotificationService notificationService, IMapper mapper, 
        IStringLocalizer<CandidateAccountController> stringLocalization)
    {
        _unitOfWork = unitOfWork;
        _notificationService = notificationService;
        _mapper = mapper;
        _stringLocalization = stringLocalization;
    }

    [HttpGet]
    public async Task<IActionResult> AllVacancies(QueryParameters queryParameters)
    {
        var username = GetUsername();
        var vacancies = await _unitOfWork.VacancyRepository
            .GetAllVacanciesAsync(queryParameters);
        var userData = await _unitOfWork.UserRepository
            .GetUserDataAsync(new() {Username = username, UserRole = ApplicationRoles.CandidateRole});

        ViewData["CandidateId"] = userData.User.CandidateUser!.CandidateUserId;

        var vacanciesViewModel = new VacanciesViewModel
        {
            QueryParameters = queryParameters,
            CurrentController = ControllerContext.RouteData.Values["controller"]?.ToString(),
            CurrentAction = ControllerContext.RouteData.Values["action"]?.ToString(),
            PageCount = vacancies.PageCount,
            VacancyCount = vacancies.VacancyCount,
            Vacancies = vacancies.VacancyList,
            Pages = vacancies.Pages,
            Username = username
        };

        vacanciesViewModel = await InitialIncludeVacanciesDataAsync(vacanciesViewModel);

        var actionResult = CheckQueryParametersForVacanciesConditions(ref queryParameters,
            vacanciesViewModel, vacancies);

        return actionResult;
    }

    [HttpGet]
    public async Task<IActionResult> RefreshCandidateInfo()
    {
        var username = GetUsername();
        var userRole = GetUserRole();
        var usernameDto = new UsernameDto {Username = username, UserRole = userRole};
        
        var userData =
            await _unitOfWork.UserRepository
                .GetUserDataAsync(usernameDto);
        
        if (!userData.IsSuccessfully)
        {
            _notificationService.CustomErrorMessage(_stringLocalization["UserNotFoundError"]);
            return View();
        }

        var refreshCandidateViewModel = _mapper.Map<RefreshCandidateInfoViewModel>(userData.User.CandidateUser);

        refreshCandidateViewModel = await InitialIncludeRefreshData(refreshCandidateViewModel);

        return View(refreshCandidateViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RefreshCandidateInfo(RefreshCandidateInfoViewModel refreshCandidateInfoViewModel)
    {
        var username = GetUsername();
        
        refreshCandidateInfoViewModel = await InitialIncludeRefreshData(refreshCandidateInfoViewModel);

        if (!ModelState.IsValid)
        {
            _notificationService.CustomErrorMessage(_stringLocalization["RefreshDetailsError"]);
            return View(refreshCandidateInfoViewModel);
        }

        var refreshCandidateDto = _mapper.Map<RefreshCandidateDto>(refreshCandidateInfoViewModel);
        var refreshCandidateResult =
            await _unitOfWork.UserRepository.RefreshCandidateUserInfoAsync(refreshCandidateDto);

        if (!refreshCandidateResult.IsSuccessfully)
        {
            _notificationService.CustomErrorMessage(_stringLocalization["RefreshDetailsError"]);
            return View(refreshCandidateInfoViewModel);
        }

        _notificationService.CustomSuccessMessage(_stringLocalization["RefreshDetailsSuccess"]);
        return RedirectToAction("Profile", "Account", new {username});
    }

    private async Task<VacanciesViewModel> InitialIncludeVacanciesDataAsync(VacanciesViewModel vacanciesViewModel)
    {
        var workCategories = await _unitOfWork.WorkCategoryRepository.GetAllWorkCategoriesAsync();
        var englishLevels = await _unitOfWork.EnglishLevelRepository.GetAllEnglishLevelsAsync();
        var typesOfCompany = await _unitOfWork.TypeOfCompanyRepository.GetAllTypesOfCompanyAsync();
        var howToWorks = await _unitOfWork.HowToWorkRepository.GetAllHowToWorkAsync();
        var candidateRegions = await _unitOfWork.CandidateRegionRepository.GetAllCandidateRegionsAsync();

        vacanciesViewModel.WorkCategories = workCategories.WorkCategories;
        vacanciesViewModel.EnglishLevels = englishLevels.EnglishLevels;
        vacanciesViewModel.TypesOfCompanies = typesOfCompany.TypesOfCompanies;
        vacanciesViewModel.HowToWorks = howToWorks.HowToWorks;
        vacanciesViewModel.CandidateRegions = candidateRegions.CandidateRegions;

        return vacanciesViewModel;
    }

    private IActionResult CheckQueryParametersForVacanciesConditions<T>(ref QueryParameters queryParameters,
        SettingsViewModel vacanciesViewModel, T repositoryResult)
        where T : ListBaseResponse
    {
        var username = GetUsername();

        if (queryParameters.PageNumber < 1)
        {
            if (vacanciesViewModel.PageCount == 0)
            {
                _notificationService.CustomErrorMessage(
                    "Вакансій за вашим запитом не знайдено або з вакансіями не було проведено жодних дій");
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

        if (queryParameters.PageNumber > repositoryResult.PageCount)
        {
            queryParameters.PageNumber = repositoryResult.PageCount;
            return RedirectToAction(vacanciesViewModel.CurrentAction, new
            {
                queryParameters.PageNumber, queryParameters.SearchString,
                queryParameters.SortBy, username, queryParameters.WorkCategory, queryParameters.EnglishLevel,
                queryParameters.CandidateRegion, queryParameters.CompanyType, queryParameters.HowToWork
            });
        }

        return View(vacanciesViewModel);
    }

    private async Task<RefreshCandidateInfoViewModel> InitialIncludeRefreshData(
        RefreshCandidateInfoViewModel refreshCandidateInfoViewModel)
    {
        var username = GetUsername();

        var englishLevelResponse = await _unitOfWork.EnglishLevelRepository.GetAllEnglishLevelsAsync();
        var workCategoryResponse = await _unitOfWork.WorkCategoryRepository.GetAllWorkCategoriesAsync();
        var communicationLanguageResponse =
            await _unitOfWork.CommunicationLanguageRepository.GetAllCommunicationLanguagesAsync();

        refreshCandidateInfoViewModel.EnglishLevels = englishLevelResponse.EnglishLevels;
        refreshCandidateInfoViewModel.WorkCategories = workCategoryResponse.WorkCategories;
        refreshCandidateInfoViewModel.CommunicationLanguages = communicationLanguageResponse.CommunicationLanguages;
        refreshCandidateInfoViewModel.Username = username;

        return refreshCandidateInfoViewModel;
    }
}