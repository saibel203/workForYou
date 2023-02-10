using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkForYou.Core.AdditionalModels;
using WorkForYou.Core.DTOModels.UserDTOs;
using WorkForYou.Core.IRepositories;
using WorkForYou.Core.IServices;
using WorkForYou.WebUI.ViewModels;
using WorkForYou.WebUI.ViewModels.Forms;

namespace WorkForYou.WebUI.Controllers;

[Authorize(Roles = "candidate")]
public class CandidateAccountController : Controller
{
    private readonly INotificationService _notificationService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CandidateAccountController(IUnitOfWork unitOfWork, INotificationService notificationService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _notificationService = notificationService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> AllVacancies(QueryParameters queryParameters)
    {
        var vacancies = await _unitOfWork.VacancyRepository.GetAllVacanciesAsync(queryParameters);
        
        var vacanciesViewModel = new VacanciesViewModel
        {
            QueryParameters = queryParameters,
            CurrentController = ControllerContext.RouteData.Values["controller"]?.ToString(),
            CurrentAction = ControllerContext.RouteData.Values["action"]?.ToString(),
            PageCount = vacancies.PageCount,
            VacancyCount = vacancies.VacancyCount,
            Vacancies = vacancies.VacancyList,
            Pages = vacancies.Pages,
            Username = User.Identity?.Name!
        };
        
        if (queryParameters.PageNumber < 1)
        {
            if (queryParameters.PageNumber == 0 && !string.IsNullOrEmpty(queryParameters.SearchString))
            {
                _notificationService.CustomErrorMessage("Вакансій за запитом не знайдено");
                return View(vacanciesViewModel);
            }
            
            queryParameters.PageNumber = 1;
            return RedirectToAction("AllVacancies", new
            {
                queryParameters.PageNumber, queryParameters.SearchString,
                queryParameters.SortBy
            });   
        }

        if (queryParameters.PageNumber > vacancies.PageCount)
        {
            queryParameters.PageNumber = vacancies.PageCount;
            return RedirectToAction("AllVacancies", new
            {
                queryParameters.PageNumber, queryParameters.SearchString,
                queryParameters.SortBy
            });   
        }

        return View(vacanciesViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> FavouriteList()
    {
        var favouriteVacancies =
            await _unitOfWork.UserRepository.ShowFavouriteListAsync(new() {Username = User.Identity?.Name!});

        return View(favouriteVacancies.FavouriteList);
    }

    [HttpGet]
    public async Task<IActionResult> AddToFavouriteList(int id)
    {
        await _unitOfWork.VacancyRepository.AddVacancyToFavouriteList(User.Identity!.Name!, id);

        return RedirectToAction("AllVacancies");
    }

    [HttpGet]
    public async Task<IActionResult> RefreshCandidateInfo()
    {
        var username = User.Identity?.Name!;
        var userData = await _unitOfWork.UserRepository.GetUserDataAsync(new() { Username = username });

        var englishLevelResponse = await _unitOfWork.EnglishLevelRepository.GetAllEnglishLevelsAsync();
        var workCategoryResponse = await _unitOfWork.WorkCategoryRepository.GetAllWorkCategoriesAsync();
        var communicationLanguageResponse =
            await _unitOfWork.CommunicationLanguageRepository.GetAllCommunicationLanguagesAsync();
        
        if (!userData.IsSuccessfully)
            return View();
        
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
            return View(refreshCandidateInfoViewModel);

        var refreshCandidateDto = _mapper.Map<RefreshCandidateDto>(refreshCandidateInfoViewModel);
        var refreshCandidateResult = await _unitOfWork.UserRepository.RefreshCandidateUserInfoAsync(refreshCandidateDto);

        if (!refreshCandidateResult.IsSuccessfully)
            return View(refreshCandidateInfoViewModel);

        return RedirectToAction("Profile", "Account", new {username});
    }
}