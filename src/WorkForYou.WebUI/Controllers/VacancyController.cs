using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkForYou.Core.AdditionalModels;
using WorkForYou.Core.DTOModels.UserDTOs;
using WorkForYou.Core.DTOModels.VacancyDTOs;
using WorkForYou.Core.IRepositories;
using WorkForYou.Core.IServices;
using WorkForYou.Core.Models;
using WorkForYou.Core.ValueObjects;
using WorkForYou.WebUI.ViewModels;
using WorkForYou.WebUI.ViewModels.Forms;

namespace WorkForYou.WebUI.Controllers;

public class VacancyController : BaseController
{
    private readonly INotificationService _notificationService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public VacancyController(IUnitOfWork unitOfWork, IMapper mapper, INotificationService notificationService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _notificationService = notificationService;
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
        var currentUser = await _unitOfWork.UserRepository.GetUserDataAsync(new() {Username = User.Identity?.Name!});

        actionVacancyViewModel = await InitActionVacancyViewModel(actionVacancyViewModel);
        actionVacancyViewModel.EmployerUser = currentUser.User.EmployerUser;

        if (!ModelState.IsValid)
        {
            _notificationService.CustomErrorMessage("Помилка при спробі створити вакансію");
            return View(actionVacancyViewModel);
        }

        var createVacancyDto = _mapper.Map<ActionVacancyDto>(actionVacancyViewModel);
        var createVacancyResult = await _unitOfWork.VacancyRepository.CreateVacancyAsync(createVacancyDto);

        if (!createVacancyResult.IsSuccessfully)
        {
            _notificationService.CustomErrorMessage("Помилка при спробі створити вакансію");
            return View(actionVacancyViewModel);
        }

        _notificationService.CustomSuccessMessage("Вакансія успішно створена");

        return RedirectToAction("AllVacancies", "EmployerAccount");
    }

    [HttpGet]
    [Authorize(Roles = "employer")]
    public async Task<IActionResult> RemoveVacancy(int id)
    {
        var vacancyData = await _unitOfWork.VacancyRepository.GetVacancyByIdAsync(id);

        if (!vacancyData.IsSuccessfully || vacancyData.Vacancy is null)
        {
            _notificationService.CustomErrorMessage(NotificationMessages.VacancyNotFoundError);
            return RedirectToAction("AllVacancies", "EmployerAccount");
        }

        var isOwnerResult = IsUserOwner(vacancyData.Vacancy);

        if (!isOwnerResult)
        {
            _notificationService.CustomErrorMessage(NotificationMessages.VacancyNotOwnerError);
            return RedirectToAction("AllVacancies", "EmployerAccount");
        }

        var removeVacancyResult = await _unitOfWork.VacancyRepository.RemoveVacancyAsync(id);

        if (!removeVacancyResult.IsSuccessfully)
        {
            _notificationService.CustomErrorMessage("Помилка при спробі видалення вакансії");
            return RedirectToAction("VacancyDetails", id);
        }

        _notificationService.CustomSuccessMessage("Вакансія успішно видалена");

        return RedirectToAction("AllVacancies", "EmployerAccount");
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> VacancyDetails(int id)
    {
        var vacancy = await _unitOfWork.VacancyRepository.GetVacancyByIdAsync(id);

        if (!vacancy.IsSuccessfully)
        {
            _notificationService.CustomErrorMessage(NotificationMessages.VacancyNotFoundError);
            return RedirectToAction("Index", "Main");
        }

        return View(vacancy.Vacancy);
    }

    [HttpGet]
    [Authorize(Roles = "employer")]
    public async Task<IActionResult> EditVacancy(int id)
    {
        var vacancyData = await _unitOfWork.VacancyRepository.GetVacancyByIdAsync(id);

        if (!vacancyData.IsSuccessfully || vacancyData.Vacancy is null)
        {
            _notificationService.CustomErrorMessage(NotificationMessages.VacancyNotFoundError);
            return RedirectToAction("AllVacancies", "EmployerAccount");
        }

        var isOwnerResult = IsUserOwner(vacancyData.Vacancy);

        if (!isOwnerResult)
        {
            _notificationService.CustomErrorMessage(NotificationMessages.VacancyNotOwnerError);
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
        var currentUser = await _unitOfWork.UserRepository.GetUserDataAsync(new() {Username = User.Identity?.Name!});

        actionVacancyViewModel = await InitActionVacancyViewModel(actionVacancyViewModel);
        actionVacancyViewModel.EmployerUser = currentUser.User.EmployerUser;

        if (!ModelState.IsValid)
        {
            _notificationService.CustomErrorMessage("Помилка при спробі редагувати вакансію");
            return View(actionVacancyViewModel);
        }

        var updateVacancyDto = _mapper.Map<ActionVacancyDto>(actionVacancyViewModel);
        var updateVacancyResult = await _unitOfWork.VacancyRepository.UpdateVacancyAsync(updateVacancyDto);

        if (!updateVacancyResult.IsSuccessfully)
        {
            _notificationService.CustomErrorMessage("Помилка при спробі редагувати вакансію");
            return View(actionVacancyViewModel);
        }

        _notificationService.CustomSuccessMessage("Вакансію успішно обновлено");

        return RedirectToAction("AllVacancies", "EmployerAccount");
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> AllEmployerVacancies(string username, QueryParameters queryParameters)
    {
        var vacancies =
            await _unitOfWork.VacancyRepository.GetAllEmployerVacanciesAsync(new UsernameDto {Username = username},
                queryParameters);

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

        if (queryParameters.PageNumber < 1)
        {
            if (queryParameters.PageNumber == 0 && !string.IsNullOrEmpty(queryParameters.SearchString))
            {
                _notificationService.CustomErrorMessage("Кандидатів за запитом не знайдено");
                return View(vacanciesViewModel);
            }
            
            queryParameters.PageNumber = 1;
            return RedirectToAction("AllEmployerVacancies", new
            {
                queryParameters.PageNumber, queryParameters.SearchString,
                queryParameters.SortBy, username
            });   
        }

        if (queryParameters.PageNumber > vacancies.PageCount)
        {
            queryParameters.PageNumber = vacancies.PageCount;
            return RedirectToAction("AllEmployerVacancies", new
            {
                queryParameters.PageNumber, queryParameters.SearchString,
                queryParameters.SortBy, username
            });   
        }
        return View(vacanciesViewModel);
    }

    private bool IsUserOwner(Vacancy vacancy)
    {
        var currentUserId = GetUserId();
        if (vacancy.EmployerUser?.ApplicationUser?.Id != currentUserId)
            return false;

        return true;
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