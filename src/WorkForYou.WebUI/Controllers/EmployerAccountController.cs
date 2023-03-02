﻿using AutoMapper;
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

[Authorize(Roles = "employer")]
public class EmployerAccountController : BaseController
{
    private readonly IStringLocalizer<EmployerAccountController> _stringLocalization;
    private readonly INotificationService _notificationService;
    private readonly IUserService _userService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public EmployerAccountController(IUnitOfWork unitOfWork, INotificationService notificationService, IMapper mapper,
        IUserService userService, IStringLocalizer<EmployerAccountController> stringLocalization)
    {
        _unitOfWork = unitOfWork;
        _notificationService = notificationService;
        _mapper = mapper;
        _userService = userService;
        _stringLocalization = stringLocalization;
    }

    [HttpGet]
    public async Task<IActionResult> AllVacancies(QueryParameters queryParameters)
    {
        var username = GetUsername();

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
                _notificationService.CustomErrorMessage(_stringLocalization["VacanciesNotFoundError"]);
                return View(vacanciesViewModel);
            }

            if (vacanciesViewModel.VacancyCount == 0)
            {
                return View(vacanciesViewModel);
            }

            queryParameters.PageNumber = 1;
            return RedirectToAction(nameof(AllVacancies),
                new
                {
                    queryParameters.PageNumber, queryParameters.SearchString, queryParameters.SortBy,
                    queryParameters.Username
                });
        }

        if (queryParameters.PageNumber > vacancies.PageCount)
        {
            queryParameters.PageNumber = vacancies.PageCount;
            return RedirectToAction(nameof(AllVacancies),
                new
                {
                    queryParameters.PageNumber, queryParameters.SearchString, queryParameters.SortBy,
                    queryParameters.Username
                });
        }

        return View(vacanciesViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> AllCandidates(QueryParameters queryParameters)
    {
        var candidatesResult =
            await _unitOfWork.UserRepository.GetAllCandidatesAsync(queryParameters);

        var username = GetUsername();
        var userData = await _unitOfWork.UserRepository.GetUserDataAsync(new() {Username = username});

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
            PageCount = candidatesResult.PageCount,
            VacancyCount = candidatesResult.VacancyCount,
            ApplicationUsers = candidatesResult.ApplicationUsers,
            Pages = candidatesResult.Pages,
            Username = GetUsername(),
            WorkCategories = workCategories.WorkCategories,
            EnglishLevels = englishLevels.EnglishLevels,
            CommunicationLanguages = communicationLanguages.CommunicationLanguages
        };

        if (queryParameters.PageNumber < 1)
        {
            if (queryParameters.PageNumber == 0 && !string.IsNullOrEmpty(queryParameters.SearchString)
                || candidatesViewModel.VacancyCount == 0 || candidatesViewModel.PageCount == 0)
            {
                _notificationService.CustomErrorMessage(_stringLocalization["CandidatesNotFoundError"]);
                return View(candidatesViewModel);
            }

            queryParameters.PageNumber = 1;
            return RedirectToAction(nameof(AllCandidates), new
            {
                queryParameters.PageNumber, queryParameters.SearchString, queryParameters.SortBy,
                queryParameters.Username, queryParameters.WorkCategory, queryParameters.EnglishLevel,
                queryParameters.CommunicationLanguages
            });
        }

        if (queryParameters.PageNumber > candidatesResult.PageCount)
        {
            queryParameters.PageNumber = candidatesResult.PageCount;
            return RedirectToAction(nameof(AllCandidates), new
            {
                queryParameters.PageNumber, queryParameters.SearchString,
                queryParameters.SortBy, queryParameters.Username, queryParameters.WorkCategory,
                queryParameters.EnglishLevel,
                queryParameters.CommunicationLanguages
            });
        }

        return View(candidatesViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> RefreshEmployerInfo()
    {
        var username = GetUsername();
        var userData = await _unitOfWork.UserRepository.GetUserDataAsync(new() {Username = username});

        if (!userData.IsSuccessfully)
        {
            _notificationService.CustomErrorMessage(_stringLocalization["UserNotFoundError"]);
            return View();
        }

        var refreshEmployerInfoVieModel = _mapper.Map<RefreshEmployerInfoViewModel>(userData.User.EmployerUser);
        refreshEmployerInfoVieModel.Username = username;

        return View(refreshEmployerInfoVieModel);
    }

    [HttpPost]
    public async Task<IActionResult> RefreshEmployerInfo(RefreshEmployerInfoViewModel refreshEmployerInfoViewModel)
    {
        var username = GetUsername();

        refreshEmployerInfoViewModel.Username = username;

        if (!ModelState.IsValid)
        {
            _notificationService.CustomErrorMessage(_stringLocalization["RefreshAccountError"]);
            return View(refreshEmployerInfoViewModel);
        }

        var refreshEmployerDto = _mapper.Map<RefreshEmployerDto>(refreshEmployerInfoViewModel);
        var refreshEmployerResult = await _unitOfWork.UserRepository.RefreshEmployerInfoAsync(refreshEmployerDto);

        if (!refreshEmployerResult.IsSuccessfully)
        {
            _notificationService.CustomErrorMessage(_stringLocalization["RefreshAccountError"]);
            return View(refreshEmployerInfoViewModel);
        }

        _notificationService.CustomSuccessMessage(_stringLocalization["RefreshAccountSuccess"]);
        return RedirectToAction("Profile", "Account", new {username});
    }

    [HttpGet]
    public async Task<IActionResult> AddToFavouriteList(int id, string? returnUrl)
    {
        var username = GetUsername();

        var addToFavouriteResult = await _userService
            .AddCandidateToFavouriteListAsync(new() {Username = username}, id);

        if (!addToFavouriteResult.IsSuccessfully)
            _notificationService.CustomErrorMessage(_stringLocalization["AddToFavouriteError"]);

        _notificationService.CustomSuccessMessage(addToFavouriteResult.Message);

        if (Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction(nameof(AllCandidates));
    }

    [HttpGet]
    public async Task<IActionResult> FavouriteList(QueryParameters queryParameters)
    {
        var username = GetUsername();
        var userData = await _unitOfWork.UserRepository.GetUserDataAsync(new() {Username = username});
        var candidates = await _unitOfWork.UserRepository
            .ShowFavouriteCandidatesListAsync(new() {Username = username}, queryParameters);

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
            CandidateUsers = candidates.FavouriteCandidates,
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
            return RedirectToAction(nameof(FavouriteList), new
            {
                queryParameters.PageNumber, queryParameters.SearchString, queryParameters.SortBy,
                queryParameters.Username, queryParameters.WorkCategory, queryParameters.EnglishLevel,
                queryParameters.CommunicationLanguages
            });
        }

        if (queryParameters.PageNumber > candidates.PageCount)
        {
            queryParameters.PageNumber = candidates.PageCount;
            return RedirectToAction(nameof(FavouriteList), new
            {
                queryParameters.PageNumber, queryParameters.SearchString,
                queryParameters.SortBy, queryParameters.Username, queryParameters.WorkCategory,
                queryParameters.EnglishLevel, queryParameters.CommunicationLanguages
            });
        }

        return View(candidatesViewModel);
    }
}