﻿using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using WorkForYou.Core.Models;

namespace WorkForYou.WebUI.Controllers;

public class BaseController : Controller
{
    private protected string GetUserId() => User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

    private protected string GetUserRole() => User.FindFirst(ClaimTypes.Role)?.Value!;

    private protected string GetUsername() => User.Identity?.Name!;
    
    private protected bool IsUserVacancyOwner(Vacancy vacancy)
    {
        var currentUserId = GetUserId();
        
        return vacancy.EmployerUser?.ApplicationUser?.Id == currentUserId;
    }
}