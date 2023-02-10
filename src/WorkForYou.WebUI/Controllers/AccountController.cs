using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkForYou.Core.IRepositories;
using WorkForYou.Core.IServices;
using WorkForYou.Core.DTOModels.UserDTOs;
using WorkForYou.Core.ValueObjects;
using WorkForYou.WebUI.ViewModels.Forms;

namespace WorkForYou.WebUI.Controllers;

[Authorize]
public class AccountController : Controller
{
    private readonly INotificationService _notificationService;
    private readonly IAuthService _authService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AccountController(IAuthService authService, IMapper mapper, IUnitOfWork unitOfWork,
        INotificationService notificationService)
    {
        _authService = authService;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _notificationService = notificationService;
    }

    [HttpGet]
    public async Task<IActionResult> ChangePassword()
    {
        var currentUser = await _unitOfWork.UserRepository.GetUserDataAsync(new() {Username = User.Identity?.Name!});

        if (!currentUser.IsSuccessfully)
            _notificationService.CustomErrorMessage(NotificationMessages.UserNotFoundError);

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
        {
            _notificationService.CustomErrorMessage("Помилка при спробі змінити пароль");
            return View(changePasswordViewModel);
        }

        var changePasswordDto = _mapper.Map<ChangePasswordDto>(changePasswordViewModel);

        var changePasswordResult = await _authService.ChangePasswordAsync(changePasswordDto);

        if (!changePasswordResult.IsSuccessfully)
        {
            if (changePasswordResult.Errors is not null)
                foreach (var error in changePasswordResult.Errors)
                    ModelState.AddModelError(error.Code, error.Description);
            else
                ModelState.AddModelError("", changePasswordResult.Message);

            _notificationService.CustomErrorMessage("Помилка при спробі змінити пароль");

            return View(changePasswordViewModel);
        }

        _notificationService.CustomSuccessMessage("Пароль успішно змінено");

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        HttpContext.Session.Clear();
        await _authService.LogoutAsync();
        _notificationService.CustomSuccessMessage("Ви успішно вийшли");
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> Profile(string username)
    {
        var user = await _unitOfWork.UserRepository.GetUserDataAsync(new() {Username = username});

        if (!user.IsSuccessfully)
        {
            _notificationService.CustomErrorMessage(NotificationMessages.UserNotFoundError);
            return RedirectToAction("Index", "Main");
        }

        return View(user.User);
    }

    [HttpPost]
    public async Task<IActionResult> ProfileUploadImage(IFormFile userFile)
    {
        var imageResult = await _unitOfWork.UserRepository.UploadUserImageAsync(userFile,
            new UsernameDto {Username = User.Identity?.Name!});

        if (!imageResult.IsSuccessfully)
            _notificationService.CustomErrorMessage("Помилка при завантажені картинки");
        
        _notificationService.CustomSuccessMessage("Картинка успішно змінена");
        return RedirectToAction("Profile", new {username = User.Identity?.Name!});
    }

    [HttpGet]
    public async Task<IActionResult> RefreshGeneralProfileInfo()
    {
        var username = User.Identity?.Name!;
        var userData = await _unitOfWork.UserRepository.GetUserDataAsync(new UsernameDto {Username = username});

        if (!userData.IsSuccessfully)
            return RedirectToAction("RefreshGeneralProfileInfo");

        var user = _mapper.Map<RefreshGeneralProfileInfoViewModel>(userData.User);

        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> RefreshGeneralProfileInfo(RefreshGeneralProfileInfoViewModel refreshGeneralProfileInfoViewModel)
    {
        if (!ModelState.IsValid)
            return View(refreshGeneralProfileInfoViewModel);

        var refreshGeneralDto = _mapper.Map<RefreshGeneralUserDto>(refreshGeneralProfileInfoViewModel);
        var refreshGeneralInfoResult = await _unitOfWork.UserRepository
            .RefreshGeneralInfoAsync(refreshGeneralDto);

        if (!refreshGeneralInfoResult.IsSuccessfully)
        {
            ModelState.AddModelError("", refreshGeneralInfoResult.Message);
            return View(refreshGeneralProfileInfoViewModel);
        }

        return RedirectToAction("Profile", new {username = User.Identity?.Name!});
    }
}