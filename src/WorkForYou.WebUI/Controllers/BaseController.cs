﻿using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using WorkForYou.Core.Models;

namespace WorkForYou.WebUI.Controllers;

public class BaseController : Controller
{
    public string GetUserId()
    {
        return User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
    }

    public bool IsUserVacancyOwner(Vacancy vacancy)
    {
        var currentUserId = GetUserId();
        
        return vacancy.EmployerUser?.ApplicationUser?.Id == currentUserId;
    }
    
    
}