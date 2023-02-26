using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WorkForYou.Core.AdditionalModels;
using WorkForYou.Core.DTOModels.UserDTOs;
using WorkForYou.Core.RepositoryInterfaces;
using WorkForYou.Core.ServiceInterfaces;
using WorkForYou.WebUI.ViewModels;
using WorkForYou.WebUI.ViewModels.Forms;

namespace WorkForYou.WebUI.Controllers;

[Authorize(Roles = "candidate")]
public class CandidateAccountController : Controller
{
    private readonly IStringLocalizer<CandidateAccountController> _stringLocalization;
    private readonly INotificationService _notificationService;
    private readonly IVacancyService _vacancyService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    private const string CandidateRole = "candidate";

    public CandidateAccountController(IUnitOfWork unitOfWork, INotificationService notificationService, IMapper mapper,
        IVacancyService vacancyService, IStringLocalizer<CandidateAccountController> stringLocalization)
    {
        _unitOfWork = unitOfWork;
        _notificationService = notificationService;
        _mapper = mapper;
        _vacancyService = vacancyService;
        _stringLocalization = stringLocalization;
    }

    [HttpGet]
    public async Task<IActionResult> AllVacancies(QueryParameters queryParameters)
    {
        var vacancies = await _unitOfWork.VacancyRepository.GetAllVacanciesAsync(queryParameters);
        var username = User.Identity?.Name!;
        var userData = await _unitOfWork.UserRepository
            .GetUserDataAsync(new() {Username = username, UserRole = CandidateRole});

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
            PageCount = vacancies.PageCount,
            VacancyCount = vacancies.VacancyCount,
            Vacancies = vacancies.VacancyList,
            Pages = vacancies.Pages,
            Username = username,
            WorkCategories = workCategories.WorkCategories,
            EnglishLevels = englishLevels.EnglishLevels,
            TypesOfCompanies = typesOfCompany.TypesOfCompanies,
            HowToWorks = howToWorks.HowToWorks,
            CandidateRegions = candidateRegions.CandidateRegions
        };

        if (queryParameters.PageNumber < 1)
        {
            if (queryParameters.PageNumber == 0 && !string.IsNullOrEmpty(queryParameters.SearchString)
                || vacanciesViewModel.VacancyCount == 0)
            {
                _notificationService.CustomErrorMessage(_stringLocalization["NotFoundError"]);
                return View(vacanciesViewModel);
            }

            queryParameters.PageNumber = 1;
            return RedirectToAction(nameof(AllVacancies), new
            {
                queryParameters.PageNumber, queryParameters.SearchString,
                queryParameters.SortBy, queryParameters.WorkCategory, queryParameters.EnglishLevel,
                queryParameters.CandidateRegion, queryParameters.CompanyType, queryParameters.HowToWork
            });
        }

        if (queryParameters.PageNumber > vacancies.PageCount)
        {
            queryParameters.PageNumber = vacancies.PageCount;
            return RedirectToAction(nameof(AllVacancies), new
            {
                queryParameters.PageNumber, queryParameters.SearchString,
                queryParameters.SortBy, queryParameters.WorkCategory, queryParameters.EnglishLevel,
                queryParameters.CandidateRegion, queryParameters.CompanyType, queryParameters.HowToWork
            });
        }

        return View(vacanciesViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> FavouriteList(QueryParameters queryParameters)
    {
        var username = User.Identity?.Name!;
        var userData = await _unitOfWork.UserRepository.GetUserDataAsync(new() {Username = username});
        var favouriteVacancies =
            await _unitOfWork.UserRepository
                .ShowFavouriteListAsync(new() {Username = username}, queryParameters);

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
            Vacancies = favouriteVacancies.FavouriteList,
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
            if (queryParameters.PageNumber == 0 && !string.IsNullOrEmpty(queryParameters.SearchString)
                || vacanciesViewModel.PageCount == 0 || vacanciesViewModel.VacancyCount == 0)
            {
                _notificationService.CustomErrorMessage(_stringLocalization["NotFoundFavourite"]);
                return View(vacanciesViewModel);
            }

            queryParameters.PageNumber = 1;
            return RedirectToAction(nameof(FavouriteList), new
            {
                queryParameters.PageNumber, queryParameters.SearchString,
                queryParameters.SortBy, username, queryParameters.WorkCategory, queryParameters.EnglishLevel,
                queryParameters.CandidateRegion, queryParameters.CompanyType, queryParameters.HowToWork
            });
        }

        if (queryParameters.PageNumber > favouriteVacancies.PageCount)
        {
            queryParameters.PageNumber = favouriteVacancies.PageCount;
            return RedirectToAction(nameof(FavouriteList), new
            {
                queryParameters.PageNumber, queryParameters.SearchString,
                queryParameters.SortBy, username, queryParameters.WorkCategory, queryParameters.EnglishLevel,
                queryParameters.CandidateRegion, queryParameters.CompanyType, queryParameters.HowToWork
            });
        }

        return View(vacanciesViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> RespondedList(QueryParameters queryParameters)
    {
        var username = User.Identity?.Name!;
        var usernameDto = new UsernameDto {Username = username, UserRole = CandidateRole};

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
                _notificationService.CustomErrorMessage(
                    "Вакансій за вашим запитом немає, або ви не відгукнулись на жодну вакансію");
                return View(vacanciesViewModel);
            }

            queryParameters.PageNumber = 1;
            return RedirectToAction(nameof(RespondedList), new
            {
                queryParameters.PageNumber, queryParameters.SearchString,
                queryParameters.SortBy, username, queryParameters.WorkCategory, queryParameters.EnglishLevel,
                queryParameters.CandidateRegion, queryParameters.CompanyType, queryParameters.HowToWork
            });
        }

        if (queryParameters.PageNumber > respondedList.PageCount)
        {
            queryParameters.PageNumber = respondedList.PageCount;
            return RedirectToAction(nameof(RespondedList), new
            {
                queryParameters.PageNumber, queryParameters.SearchString,
                queryParameters.SortBy, username, queryParameters.WorkCategory, queryParameters.EnglishLevel,
                queryParameters.CandidateRegion, queryParameters.CompanyType, queryParameters.HowToWork
            });
        }

        return View(vacanciesViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> AddToFavouriteList(int id, string? returnUrl)
    {
        var username = User.Identity?.Name!;

        var addToFavouriteResult = await _vacancyService
            .AddVacancyToFavouriteListAsync(new() {Username = username, UserRole = CandidateRole}, id);

        if (!addToFavouriteResult.IsSuccessfully)
            _notificationService.CustomErrorMessage(_stringLocalization["AddToFavouriteError"]);

        _notificationService.CustomSuccessMessage(addToFavouriteResult.Message);

        if (Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction(nameof(AllVacancies));
    }

    [HttpGet]
    public async Task<IActionResult> RefreshCandidateInfo()
    {
        var username = User.Identity?.Name!;
        var userData =
            await _unitOfWork.UserRepository.GetUserDataAsync(new() {Username = username, UserRole = CandidateRole});

        var englishLevelResponse = await _unitOfWork.EnglishLevelRepository.GetAllEnglishLevelsAsync();
        var workCategoryResponse = await _unitOfWork.WorkCategoryRepository.GetAllWorkCategoriesAsync();
        var communicationLanguageResponse =
            await _unitOfWork.CommunicationLanguageRepository.GetAllCommunicationLanguagesAsync();

        if (!userData.IsSuccessfully)
        {
            _notificationService.CustomErrorMessage(_stringLocalization["UserNotFoundError"]);
            return View();
        }

        var refreshCandidateViewModel = _mapper.Map<RefreshCandidateInfoViewModel>(userData.User.CandidateUser);

        refreshCandidateViewModel.EnglishLevels = englishLevelResponse.EnglishLevels;
        refreshCandidateViewModel.WorkCategories = workCategoryResponse.WorkCategories;
        refreshCandidateViewModel.CommunicationLanguages = communicationLanguageResponse.CommunicationLanguages;
        refreshCandidateViewModel.Username = username;

        return View(refreshCandidateViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> RefreshCandidateInfo(RefreshCandidateInfoViewModel refreshCandidateInfoViewModel)
    {
        var username = User.Identity?.Name!;

        var englishLevelResponse = await _unitOfWork.EnglishLevelRepository.GetAllEnglishLevelsAsync();
        var workCategoryResponse = await _unitOfWork.WorkCategoryRepository.GetAllWorkCategoriesAsync();
        var communicationLanguageResponse =
            await _unitOfWork.CommunicationLanguageRepository.GetAllCommunicationLanguagesAsync();

        refreshCandidateInfoViewModel.EnglishLevels = englishLevelResponse.EnglishLevels;
        refreshCandidateInfoViewModel.WorkCategories = workCategoryResponse.WorkCategories;
        refreshCandidateInfoViewModel.CommunicationLanguages = communicationLanguageResponse.CommunicationLanguages;
        refreshCandidateInfoViewModel.Username = username;

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
}