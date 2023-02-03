using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkForYou.Core.IRepositories;
using WorkForYou.Core.IServices;
using WorkForYou.Core.DtoModels;
using WorkForYou.WebUI.ViewModels;

namespace WorkForYou.WebUI.Controllers;

[Authorize]
public class AccountController : Controller
{
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public AccountController(IAuthService authService, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _authService = authService;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<IActionResult> ChangePassword()
    {
        var currentUser = await _unitOfWork.UserRepository.GetUserDataAsync(new() {Username = User.Identity?.Name!});
        ViewData["CurrentEmail"] = currentUser.User.Email;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
    {
        var changePasswordUsernameDto = _mapper.Map<UsernameDto>(changePasswordViewModel);

        var currentUser = await _unitOfWork.UserRepository.GetUserDataAsync(changePasswordUsernameDto);
        ViewData["CurrentEmail"] = currentUser.User.Email;

        if (!ModelState.IsValid)
            return View(changePasswordViewModel);

        var changePasswordDto = _mapper.Map<ChangePasswordDto>(changePasswordViewModel);

        var changePasswordResult = await _authService.ChangePasswordAsync(changePasswordDto);

        if (!changePasswordResult.IsSuccessfully)
        {
            if (changePasswordResult.Errors is not null)
                foreach (var error in changePasswordResult.Errors)
                    ModelState.AddModelError(error.Code, error.Description);
            else
                ModelState.AddModelError("", changePasswordResult.Message);

            return View(changePasswordViewModel);
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        HttpContext.Session.Clear();
        await _authService.LogoutAsync();
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        var user = await _unitOfWork.UserRepository.GetUserDataAsync(new() {Username = User.Identity?.Name!});

        if (!user.IsSuccessfully)
            return RedirectToAction("Logout");

        return View(user.User);
    }

    [HttpGet]
    public async Task<IActionResult> UpdateAccount(int id)
    {
        var userData = await _unitOfWork.UserRepository.GetUserDataAsync(new() {Username = User.Identity?.Name!});

        var vacancyDomains = await _unitOfWork.VacancyDomainRepository.GetAllVacancyDomainsAsync();
        var workCategories = await _unitOfWork.WorkCategoryRepository.GetAllWorkCategoriesAsync();
        var candidateRegions = await _unitOfWork.CandidateRegionRepository.GetAllCandidateRegionsAsync();
        var relocates = await _unitOfWork.RelocateRepository.GetAllRelocatesAsync();
        var englishLevels = await _unitOfWork.EnglishLevelRepository.GetAllEnglishLevelsAsync();
        var typesOfCompany = await _unitOfWork.TypeOfCompanyRepository.GetAllTypesOfCompanyAsync();
        var howToWorks = await _unitOfWork.HowToWorkRepository.GetAllHowToWorkAsync();

        var userUpdateViewModel = new UserUpdateViewModel
        {
            VacancyDomains = vacancyDomains.VacancyDomains,
            WorkCategories = workCategories.WorkCategories,
            CandidateRegions = candidateRegions.CandidateRegions,
            Relocates = relocates.Relocates,
            EnglishLevels = englishLevels.EnglishLevels,
            TypesOfCompanies = typesOfCompany.TypesOfCompanies,
            HowToWorks = howToWorks.HowToWorks
        };

        return View(userUpdateViewModel);
    }
}