using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WorkForYou.Core.AdditionalModels;
using WorkForYou.Core.RepositoryInterfaces;
using WorkForYou.Core.ServiceInterfaces;
using WorkForYou.Core.DTOModels.UserDTOs;
using WorkForYou.Core.Responses.Repositories;
using WorkForYou.Shared.ViewModels.Forms;

namespace WorkForYou.WebUI.Controllers;

[Authorize]
public class AccountController : BaseController
{
    private readonly IStringLocalizer<AccountController> _stringLocalization;
    private readonly INotificationService _notificationService;
    private readonly IViewCounterService _viewCounterService;
    private readonly IFileService _fileService;
    private readonly IAuthService _authService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AccountController(IAuthService authService, IMapper mapper, IUnitOfWork unitOfWork,
        INotificationService notificationService, IStringLocalizer<AccountController> stringLocalization,
        IFileService fileService, IViewCounterService viewCounterService)
    {
        _authService = authService;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _notificationService = notificationService;
        _stringLocalization = stringLocalization;
        _fileService = fileService;
        _viewCounterService = viewCounterService;
    }

    [HttpGet]
    public async Task<IActionResult> ChangePassword()
    {
        var currentUsername = GetUsername();
        var currentRole = GetUserRole();
        var usernameDto = new UsernameDto {Username = currentUsername, UserRole = currentRole};
        
        var currentUser = await _unitOfWork.UserRepository
            .GetUserDataAsync(usernameDto);

        if (!currentUser.IsSuccessfully)
            _notificationService.CustomErrorMessage(_stringLocalization["UserNotFound"]);

        ViewData["CurrentEmail"] = currentUser.User.Email;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
    {
        var currentUsername = GetUsername();
        var currentUser = await _unitOfWork.UserRepository
            .GetUserDataAsync(new() {Username = currentUsername});
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
        var userRole = GetUserRole();
        
        UserResponse userData;
        var usernameDto = new UsernameDto {Username = username, UserRole = userRole};
        var usernameCandidateDto = new UsernameDto {Username = username, UserRole = ApplicationRoles.CandidateRole};
        var usernameEmployerDto = new UsernameDto {Username = username, UserRole = ApplicationRoles.EmployerRole};
        var isUserResult = await _authService.IsUserCandidate(usernameDto);

        if (isUserResult.IsUserCandidate)
        {
            userData = await _unitOfWork.UserRepository
                .GetUserDataAsync(usernameCandidateDto);

            const string sessionNameTemplate = "IsShowCandidate";

            if (!HttpContext.Session.Keys.Contains(sessionNameTemplate + userData.User.CandidateUser!.CandidateUserId)
                && HttpContext.User.IsInRole(ApplicationRoles.EmployerRole))
            {
                HttpContext.Session.SetString(sessionNameTemplate + userData.User.CandidateUser!.CandidateUserId, "1");

                var addViewCountResult = await _viewCounterService
                    .UpdateCandidateViewNumberCountAsync(usernameDto);

                if (!addViewCountResult.IsSuccessfully)
                    return RedirectToAction("Index", "Main");
            }
        }
        else
            userData = await _unitOfWork.UserRepository
                .GetUserDataAsync(usernameEmployerDto);

        if (!userData.IsSuccessfully)
        {
            _notificationService.CustomErrorMessage(_stringLocalization["UserNotFound"]);
            return RedirectToAction("Index", "Main");
        }

        return View(userData.User);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ProfileUploadImage(IFormFile userFile)
    {
        var username = GetUsername();
        var userRole = GetUserRole();
        var usernameDto = new UsernameDto {Username = username, UserRole = userRole};

        var uploadImageResult = await _fileService.UploadUserImageAsync(userFile, usernameDto);

        if (!uploadImageResult.IsSuccessfully)
            _notificationService.CustomErrorMessage(_stringLocalization["UploadImageError"]);

        _notificationService.CustomSuccessMessage(_stringLocalization["UploadImageSuccess"]);
        return RedirectToAction(nameof(Profile), new {username});
    }

    [HttpGet]
    public async Task<IActionResult> RefreshGeneralProfileInfo()
    {
        var username = GetUsername();
        var userRole = GetUserRole();
        var usernameDto = new UsernameDto {Username = username, UserRole = userRole};

        var userData = await _unitOfWork.UserRepository
            .GetUserDataAsync(usernameDto);

        if (!userData.IsSuccessfully)
        {
            _notificationService.CustomErrorMessage(_stringLocalization["UserNotFound"]);
            return RedirectToAction("RefreshGeneralProfileInfo");
        }

        var user = _mapper.Map<RefreshGeneralProfileInfoViewModel>(userData.User);

        return View(user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RefreshGeneralProfileInfo(
        RefreshGeneralProfileInfoViewModel refreshGeneralProfileInfoViewModel)
    {
        if (!ModelState.IsValid)
        {
            _notificationService.CustomErrorMessage(_stringLocalization["RefreshProfileError"]);
            return View(refreshGeneralProfileInfoViewModel);
        }

        var username = GetUsername();
        var userRole = GetUserRole();

        refreshGeneralProfileInfoViewModel.UserRole = userRole;

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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveUser()
    {
        var username = GetUsername();
        var userRole = GetUserRole();
        var usernameDto = new UsernameDto {Username = username, UserRole = userRole};
        var removeUserResult = await _unitOfWork.UserRepository
            .RemoveUserAsync(usernameDto);

        if (!removeUserResult.IsSuccessfully)
        {
            _notificationService.CustomErrorMessage(_stringLocalization["RemoveUserError"]);
            return RedirectToAction(nameof(Profile), new {username});
        }

        _notificationService.CustomSuccessMessage(_stringLocalization["RemoveUserSuccess"]);
        return RedirectToAction("Logout");
    }
}