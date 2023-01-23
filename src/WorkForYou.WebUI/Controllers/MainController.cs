using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WorkForYou.WebUI.Controllers;

[Authorize]
public class MainController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}