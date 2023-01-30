using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkForYou.Core.IServices;
using WorkForYou.Data.DtoModels;
using WorkForYou.WebUI.ViewModels;

namespace WorkForYou.WebUI.Controllers;

[Authorize]
public class AccountController : Controller
{
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;

    public AccountController(IAuthService authService, IMapper mapper)
    {
        _authService = authService;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult ChangePassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
    {
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
        await _authService.LogoutAsync();
        return RedirectToAction("Index", "Home");
    }
}