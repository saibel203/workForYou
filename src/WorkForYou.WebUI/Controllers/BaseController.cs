using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace WorkForYou.WebUI.Controllers;

public class BaseController : Controller
{
    public string GetUserId()
    {
        return User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
    }
}