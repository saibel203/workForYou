using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WorkForYou.Core.IServices;
using WorkForYou.Core.DtoModels;
using WorkForYou.WebUI.Attributes;
using WorkForYou.WebUI.ViewModels;

namespace WorkForYou.WebUI.Controllers;

[Unauthorized]
public class AuthController : Controller
{
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;

    public AuthController(IMapper mapper, IAuthService authService)
    {
        _mapper = mapper;
        _authService = authService;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
    {
        if (!ModelState.IsValid)
            return View(registerViewModel);

        var registerModelDto = _mapper.Map<UserRegisterDto>(registerViewModel);

        var registerResult = await _authService.RegisterAsync(registerModelDto);

        if (!registerResult.IsSuccessfully)
        {
            if (registerResult.Errors is not null)
                foreach (var error in registerResult.Errors)
                    ModelState.AddModelError(error.Code, error.Description);
            else
                ModelState.AddModelError("", registerResult.Message);

            return View(registerViewModel);
        }

        return RedirectToAction("ConfirmEmail");
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel, string? returnUrl)
    {
        returnUrl ??= Url.Content("~/");

        if (!ModelState.IsValid)
            return View(loginViewModel);

        var loginModelDto = _mapper.Map<UserLoginDto>(loginViewModel);

        var loginResult = await _authService.LoginAsync(loginModelDto);

        if (!loginResult.IsSuccessfully)
        {
            ModelState.AddModelError("", loginResult.Message);
            return View(loginViewModel);
        }

        return RedirectToLocal(returnUrl);
    }

    [HttpGet]
    public async Task<IActionResult> ConfirmEmailResult(string userId, string token)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
            return RedirectToAction("Index", "Home");

        var result = await _authService.ConfirmEmailAsync(userId, token);

        if (!result.IsSuccessfully)
        {
            if (result.Errors is not null)
                foreach (var error in result.Errors)
                    ModelState.AddModelError(error.Code, error.Description);
            else
                ModelState.AddModelError("", result.Message);

            return View();
        }

        return View();
    }

    [HttpGet]
    public IActionResult ConfirmEmail()
    {
        return View();
    }

    public IActionResult RemindPasswordResult()
    {
        return View();
    }

    [HttpGet]
    public IActionResult RemindPassword()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemindPassword(RemindPasswordViewModel forgetPasswordViewModel)
    {
        if (string.IsNullOrWhiteSpace(forgetPasswordViewModel.Email))
            return View(forgetPasswordViewModel);

        var forgetPasswordDto = _mapper.Map<RemindPasswordDto>(forgetPasswordViewModel);

        var forgetPasswordResult = await _authService.RemindPasswordAsync(forgetPasswordDto);

        if (!forgetPasswordResult.IsSuccessfully)
        {
            ModelState.AddModelError("", forgetPasswordResult.Message);

            return View(forgetPasswordViewModel);
        }

        return RedirectToAction("RemindPasswordResult");
    }

    [HttpGet]
    public IActionResult ResetPasswordResult()
    {
        return View();
    }

    [HttpGet]
    public IActionResult ResetPassword(string email, string token)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(token))
            return RedirectToAction("Index", "Home");

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
    {
        if (!ModelState.IsValid)
            return View(resetPasswordViewModel);

        var resetPasswordDto = _mapper.Map<ResetPasswordDto>(resetPasswordViewModel);

        var resetPasswordResult = await _authService.ResetPasswordAsync(resetPasswordDto);

        if (!resetPasswordResult.IsSuccessfully)
        {
            if (resetPasswordResult.Errors is not null)
                foreach (var error in resetPasswordResult.Errors)
                    ModelState.AddModelError(error.Code, error.Description);
            else
                ModelState.AddModelError("", resetPasswordResult.Message);

            return View(resetPasswordViewModel);
        }

        return RedirectToAction("ResetPasswordResult");
    }

    private IActionResult RedirectToLocal(string? returnUrl)
    {
        if (Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction("Index", "Main");
    }
}