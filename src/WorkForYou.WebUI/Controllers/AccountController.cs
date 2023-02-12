using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WorkForYou.Core.IRepositories;
using WorkForYou.Core.IServices;
using WorkForYou.Core.DTOModels.UserDTOs;
using WorkForYou.WebUI.ViewModels.Forms;

namespace WorkForYou.WebUI.Controllers;

[Authorize]
public class AccountController : Controller
{
    private readonly INotificationService _notificationService;
    private readonly IStringLocalizer<AccountController> _stringLocalization;
    private readonly IAuthService _authService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AccountController(IAuthService authService, IMapper mapper, IUnitOfWork unitOfWork,
        INotificationService notificationService, IStringLocalizer<AccountController> stringLocalization)
    {
        _authService = authService;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _notificationService = notificationService;
        _stringLocalization = stringLocalization;
    }

    [HttpGet]
    public async Task<IActionResult> ChangePassword()
    {
        var currentUser = await _unitOfWork.UserRepository.GetUserDataAsync(new() {Username = User.Identity?.Name!});

        if (!currentUser.IsSuccessfully)
            _notificationService.CustomErrorMessage(_stringLocalization["UserNotFound"]);

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
            _notificationService.CustomErrorMessage(_stringLocalization["ChangePasswordError"]);
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

            _notificationService.CustomErrorMessage(_stringLocalization["ChangePasswordError"]);

            return View(changePasswordViewModel);
        }

        _notificationService.CustomSuccessMessage(_stringLocalization["ChangePasswordSuccess"]);

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        HttpContext.Session.Clear();
        await _authService.LogoutAsync();
        _notificationService.CustomSuccessMessage(_stringLocalization["LogoutSuccess"]);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> Profile(string username)
    {
        var user = await _unitOfWork.UserRepository.GetUserDataAsync(new() {Username = username});

        if (!user.IsSuccessfully)
        {
            _notificationService.CustomErrorMessage(_stringLocalization["UserNotFound"]);
            return RedirectToAction("Index", "Main");
        }

        return View(user.User);
    }

    [HttpPost]
    public async Task<IActionResult> ProfileUploadImage(IFormFile userFile)
    {
        var username = User.Identity?.Name!;
        var imageResult = await _unitOfWork.UserRepository.UploadUserImageAsync(userFile,
            new UsernameDto {Username = User.Identity?.Name!});

        if (!imageResult.IsSuccessfully)
            _notificationService.CustomErrorMessage(_stringLocalization["UploadImageError"]);

        _notificationService.CustomSuccessMessage(_stringLocalization["UploadImageSuccess"]);
        return RedirectToAction(nameof(Profile), new {username});
    }

    [HttpGet]
    public async Task<IActionResult> RefreshGeneralProfileInfo()
    {
        var username = User.Identity?.Name!;
        var userData = await _unitOfWork.UserRepository.GetUserDataAsync(new UsernameDto {Username = username});

        if (!userData.IsSuccessfully)
        {
            _notificationService.CustomErrorMessage(_stringLocalization["UserNotFound"]);
            return RedirectToAction("RefreshGeneralProfileInfo");
        }

        var user = _mapper.Map<RefreshGeneralProfileInfoViewModel>(userData.User);

        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> RefreshGeneralProfileInfo(
        RefreshGeneralProfileInfoViewModel refreshGeneralProfileInfoViewModel)
    {
        if (!ModelState.IsValid)
        {
            _notificationService.CustomErrorMessage(_stringLocalization["RefreshProfileError"]);
            return View(refreshGeneralProfileInfoViewModel);
        }

        var username = User.Identity?.Name!;
        var refreshGeneralDto = _mapper.Map<RefreshGeneralUserDto>(refreshGeneralProfileInfoViewModel);
        var refreshGeneralInfoResult = await _unitOfWork.UserRepository
            .RefreshGeneralInfoAsync(refreshGeneralDto);

        if (!refreshGeneralInfoResult.IsSuccessfully)
        {
            _notificationService.CustomErrorMessage(_stringLocalization["RefreshProfileError"]);
            ModelState.AddModelError("", refreshGeneralInfoResult.Message);
            return View(refreshGeneralProfileInfoViewModel);
        }

        _notificationService.CustomSuccessMessage(_stringLocalization["RefreshProfileSuccess"]);
        return RedirectToAction(nameof(Profile), new {username});
    }

    [HttpGet]
    public async Task<IActionResult> RemoveUser()
    {
        var username = User.Identity?.Name!;
        var removeUserResult = await _unitOfWork.UserRepository.RemoveUserAsync(new() {Username = username});

        if (!removeUserResult.IsSuccessfully)
        {
            _notificationService.CustomErrorMessage(_stringLocalization["RemoveUserError"]);
            return RedirectToAction(nameof(Profile), new {username});
        }

        _notificationService.CustomSuccessMessage(_stringLocalization["RemoveUserSuccess"]);
        return RedirectToAction("Logout");
    }
}