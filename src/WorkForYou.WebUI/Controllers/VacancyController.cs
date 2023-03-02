using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WorkForYou.Core.AdditionalModels;
using WorkForYou.Core.DTOModels.UserDTOs;
using WorkForYou.Core.DTOModels.VacancyDTOs;
using WorkForYou.Core.RepositoryInterfaces;
using WorkForYou.Core.ServiceInterfaces;
using WorkForYou.WebUI.ViewModels;
using WorkForYou.WebUI.ViewModels.Forms;

namespace WorkForYou.WebUI.Controllers;

public class VacancyController : BaseController
{
    private readonly IStringLocalizer<VacancyController> _stringLocalization;
    private readonly INotificationService _notificationService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public VacancyController(IUnitOfWork unitOfWork, IMapper mapper, INotificationService notificationService,
        IStringLocalizer<VacancyController> stringLocalization)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _notificationService = notificationService;
        _stringLocalization = stringLocalization;
    }

    [HttpGet]
    [Authorize(Roles = "employer")]
    public async Task<IActionResult> CreateVacancy()
    {
        var actionVacancyViewModel = await InitActionVacancyViewModel();
        return View(actionVacancyViewModel);
    }

    [HttpPost]
    [Authorize(Roles = "employer")]
    public async Task<IActionResult> CreateVacancy(ActionVacancyViewModel actionVacancyViewModel)
    {
        var currentUsername = GetUsername();
        var currentUser = await _unitOfWork.UserRepository
            .GetUserDataAsync(new() {Username = currentUsername});

        actionVacancyViewModel = await InitActionVacancyViewModel(actionVacancyViewModel);
        actionVacancyViewModel.EmployerUser = currentUser.User.EmployerUser;

        if (!ModelState.IsValid)
        {
            _notificationService.CustomErrorMessage(_stringLocalization["CreateVacancyError"]);
            return View(actionVacancyViewModel);
        }

        var createVacancyDto = _mapper.Map<ActionVacancyDto>(actionVacancyViewModel);
        var createVacancyResult = await _unitOfWork.VacancyRepository.CreateVacancyAsync(createVacancyDto);

        if (!createVacancyResult.IsSuccessfully)
        {
            _notificationService.CustomErrorMessage(_stringLocalization["CreateVacancyError"]);
            return View(actionVacancyViewModel);
        }

        _notificationService.CustomSuccessMessage(_stringLocalization["CreateVacancySuccess"]);

        return RedirectToAction("AllVacancies", "EmployerAccount");
    }

    [HttpGet]
    [Authorize(Roles = "employer")]
    public async Task<IActionResult> RemoveVacancy(int id)
    {
        var vacancyData = await _unitOfWork.VacancyRepository.GetVacancyByIdAsync(id);

        if (!vacancyData.IsSuccessfully || vacancyData.Vacancy is null)
        {
            _notificationService.CustomErrorMessage(_stringLocalization["VacancyNotFoundError"]);
            return RedirectToAction("AllVacancies", "EmployerAccount");
        }

        var isOwnerResult = IsUserVacancyOwner(vacancyData.Vacancy);

        if (!isOwnerResult)
        {
            _notificationService.CustomErrorMessage(_stringLocalization["NotOwnerError"]);
            return RedirectToAction("AllVacancies", "EmployerAccount");
        }

        var removeVacancyResult = await _unitOfWork.VacancyRepository.RemoveVacancyAsync(id);

        if (!removeVacancyResult.IsSuccessfully)
        {
            _notificationService.CustomErrorMessage(_stringLocalization["RemoveVacancyError"]);
            return RedirectToAction("VacancyDetails", id);
        }

        _notificationService.CustomSuccessMessage(_stringLocalization["RemoveVacancySuccess"]);
        return RedirectToAction("AllVacancies", "EmployerAccount");
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> VacancyDetails(int id)
    {
        var vacancy = await _unitOfWork.VacancyRepository.GetVacancyByIdAsync(id);
        VacancyViewModel vacancyViewModel;

        if (!vacancy.IsSuccessfully)
        {
            _notificationService.CustomErrorMessage(_stringLocalization["VacancyNotFoundError"]);
            return RedirectToAction("Index", "Main");
        }

        if (!HttpContext.Session.Keys.Contains($"IsShowVacancy{id}")
            && HttpContext.User.IsInRole(CandidateRole))
        {
            HttpContext.Session.SetString($"IsShowVacancy{id}", "1");

            var addViewCountResult = await _unitOfWork.VacancyRepository.UpdateViewNumberOfCountAsync(id);

            if (!addViewCountResult.IsSuccessfully)
                return RedirectToAction("Index", "Main");
        }

        if (HttpContext.User.IsInRole(CandidateRole))
        {
            var username = GetUsername();
            var usernameDto = new UsernameDto {Username = username};
            var userData = await _unitOfWork.UserRepository.GetUserDataAsync(usernameDto);

            var isVacancyInRespondedList = await _unitOfWork.RespondedListRepository
                .IsVacancyInRespondedListAsync(userData.User.Id, id);

            vacancyViewModel = new VacancyViewModel
            {
                Vacancy = vacancy.Vacancy,
                IsVacancyInRespondedList = isVacancyInRespondedList.IsVacancyInFavouriteList
            };

            return View(vacancyViewModel);
        }

        vacancyViewModel = new VacancyViewModel
        {
            Vacancy = vacancy.Vacancy
        };

        return View(vacancyViewModel);
    }

    [HttpGet]
    [Authorize(Roles = "employer")]
    public async Task<IActionResult> EditVacancy(int id)
    {
        var vacancyData = await _unitOfWork.VacancyRepository.GetVacancyByIdAsync(id);

        if (!vacancyData.IsSuccessfully || vacancyData.Vacancy is null)
        {
            _notificationService.CustomErrorMessage(_stringLocalization["VacancyNotFoundError"]);
            return RedirectToAction("AllVacancies", "EmployerAccount");
        }

        var isOwnerResult = IsUserVacancyOwner(vacancyData.Vacancy);

        if (!isOwnerResult)
        {
            _notificationService.CustomErrorMessage(_stringLocalization["NotOwnerError"]);
            return RedirectToAction("AllVacancies", "EmployerAccount");
        }

        var vacancyResult = _mapper.Map<ActionVacancyViewModel>(vacancyData.Vacancy);
        vacancyResult = await InitActionVacancyViewModel(vacancyResult);
        vacancyResult.VacancyId = vacancyData.Vacancy.VacancyId;

        return View(vacancyResult);
    }

    [HttpPost]
    [Authorize(Roles = "employer")]
    public async Task<IActionResult> EditVacancy(ActionVacancyViewModel actionVacancyViewModel)
    {
        var currentUsername = GetUsername();
        var currentUser = await _unitOfWork.UserRepository
            .GetUserDataAsync(new() {Username = currentUsername});

        actionVacancyViewModel = await InitActionVacancyViewModel(actionVacancyViewModel);
        actionVacancyViewModel.EmployerUser = currentUser.User.EmployerUser;

        if (!ModelState.IsValid)
        {
            _notificationService.CustomErrorMessage(_stringLocalization["EditVacancyError"]);
            return View(actionVacancyViewModel);
        }

        var updateVacancyDto = _mapper.Map<ActionVacancyDto>(actionVacancyViewModel);
        var updateVacancyResult = await _unitOfWork.VacancyRepository.UpdateVacancyAsync(updateVacancyDto);

        if (!updateVacancyResult.IsSuccessfully)
        {
            _notificationService.CustomErrorMessage(_stringLocalization["EditVacancyError"]);
            return View(actionVacancyViewModel);
        }

        _notificationService.CustomSuccessMessage(_stringLocalization["EditVacancySuccess"]);

        return RedirectToAction("AllVacancies", "EmployerAccount");
    }

    [HttpGet]
    [Authorize(Roles = "candidate")]
    public async Task<IActionResult> AllEmployerVacancies(string username, QueryParameters queryParameters)
    {
        var currentUsername = GetUsername();
        var currentUsernameDto = new UsernameDto {Username = currentUsername, UserRole = CandidateRole};

        var userData = await _unitOfWork.UserRepository.GetUserDataAsync(currentUsernameDto);

        var vacancies =
            await _unitOfWork.VacancyRepository.GetAllEmployerVacanciesAsync(new UsernameDto {Username = username},
                queryParameters);

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
            if (queryParameters.PageNumber == 0 && !string.IsNullOrEmpty(queryParameters.SearchString))
            {
                _notificationService.CustomErrorMessage(_stringLocalization["VacanciesNotFoundError"]);
                return View(vacanciesViewModel);
            }

            queryParameters.PageNumber = 1;
            return RedirectToAction("AllEmployerVacancies", new
            {
                queryParameters.PageNumber, queryParameters.SearchString,
                queryParameters.SortBy, username, queryParameters.WorkCategory, queryParameters.EnglishLevel,
                queryParameters.CandidateRegion, queryParameters.CompanyType, queryParameters.HowToWork
            });
        }

        if (queryParameters.PageNumber > vacancies.PageCount)
        {
            queryParameters.PageNumber = vacancies.PageCount;
            return RedirectToAction("AllEmployerVacancies", new
            {
                queryParameters.PageNumber, queryParameters.SearchString,
                queryParameters.SortBy, Username = username, queryParameters.WorkCategory, queryParameters.EnglishLevel,
                queryParameters.CandidateRegion, queryParameters.CompanyType, queryParameters.HowToWork
            });
        }

        return View(vacanciesViewModel);
    }

    [HttpGet]
    [Authorize(Roles = "candidate")]
    public async Task<IActionResult> RespondedVacancy(int vacancyId)
    {
        var username = GetUsername();

        var respondedResult = await _unitOfWork.RespondedListRepository
            .RespondToVacancyAsync(new() {Username = username, UserRole = CandidateRole}, vacancyId);

        if (!respondedResult.IsSuccessfully)
        {
            _notificationService.CustomErrorMessage("При спробі відгукнутися на вакансію відбулася помилка");
            return RedirectToAction(nameof(VacancyDetails), new {id = vacancyId});
        }

        _notificationService.CustomSuccessMessage("Ви успішно відгукнулися на вакансію");
        return RedirectToAction(nameof(VacancyDetails), new {id = vacancyId});
    }

    [HttpGet]
    [Authorize(Roles = "candidate")]
    public async Task<IActionResult> RemoveRespondVacancy(int vacancyId)
    {
        var username = GetUsername();
        var usernameDto = new UsernameDto {Username = username};

        var removeRespondResult = await _unitOfWork.RespondedListRepository
            .RemoveRespondToVacancyAsync(usernameDto, vacancyId);

        if (!removeRespondResult.IsSuccessfully)
            _notificationService.CustomErrorMessage("При спробі відмінити відгук на вакансію відбулася помилка");

        _notificationService.CustomSuccessMessage("Ви успішно відмінили відгук на вакансію");
        return RedirectToAction(nameof(VacancyDetails), new {id = vacancyId});
    }

    [HttpGet]
    [Authorize(Roles = "employer")]
    public async Task<IActionResult> AllVacancyResponses(QueryParameters queryParameters, int vacancyId)
    {
        var username = GetUsername();
        var usernameDto = new UsernameDto {Username = username};
        var userData = await _unitOfWork.UserRepository.GetUserDataAsync(usernameDto);

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
                _notificationService.CustomErrorMessage("Error getting vacancies");
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

    private async Task<ActionVacancyViewModel> InitActionVacancyViewModel()
    {
        var vacancyDomains = await _unitOfWork.VacancyDomainRepository.GetAllVacancyDomainsAsync();
        var workCategories = await _unitOfWork.WorkCategoryRepository.GetAllWorkCategoriesAsync();
        var candidateRegions = await _unitOfWork.CandidateRegionRepository.GetAllCandidateRegionsAsync();
        var relocates = await _unitOfWork.RelocateRepository.GetAllRelocatesAsync();
        var englishLevels = await _unitOfWork.EnglishLevelRepository.GetAllEnglishLevelsAsync();
        var typesOfCompany = await _unitOfWork.TypeOfCompanyRepository.GetAllTypesOfCompanyAsync();
        var howToWorks = await _unitOfWork.HowToWorkRepository.GetAllHowToWorkAsync();

        var actionVacancyViewModel = new ActionVacancyViewModel
        {
            VacancyDomains = vacancyDomains.VacancyDomains,
            WorkCategories = workCategories.WorkCategories,
            CandidateRegions = candidateRegions.CandidateRegions,
            Relocates = relocates.Relocates,
            EnglishLevels = englishLevels.EnglishLevels,
            TypesOfCompanies = typesOfCompany.TypesOfCompanies,
            HowToWorks = howToWorks.HowToWorks
        };

        return actionVacancyViewModel;
    }

    private async Task<ActionVacancyViewModel> InitActionVacancyViewModel(ActionVacancyViewModel actionVacancyViewModel)
    {
        var vacancyDomains = await _unitOfWork.VacancyDomainRepository.GetAllVacancyDomainsAsync();
        var workCategories = await _unitOfWork.WorkCategoryRepository.GetAllWorkCategoriesAsync();
        var candidateRegions = await _unitOfWork.CandidateRegionRepository.GetAllCandidateRegionsAsync();
        var relocates = await _unitOfWork.RelocateRepository.GetAllRelocatesAsync();
        var englishLevels = await _unitOfWork.EnglishLevelRepository.GetAllEnglishLevelsAsync();
        var typesOfCompany = await _unitOfWork.TypeOfCompanyRepository.GetAllTypesOfCompanyAsync();
        var howToWorks = await _unitOfWork.HowToWorkRepository.GetAllHowToWorkAsync();

        actionVacancyViewModel.VacancyDomains = vacancyDomains.VacancyDomains;
        actionVacancyViewModel.WorkCategories = workCategories.WorkCategories;
        actionVacancyViewModel.CandidateRegions = candidateRegions.CandidateRegions;
        actionVacancyViewModel.Relocates = relocates.Relocates;
        actionVacancyViewModel.EnglishLevels = englishLevels.EnglishLevels;
        actionVacancyViewModel.TypesOfCompanies = typesOfCompany.TypesOfCompanies;
        actionVacancyViewModel.HowToWorks = howToWorks.HowToWorks;

        return actionVacancyViewModel;
    }
}