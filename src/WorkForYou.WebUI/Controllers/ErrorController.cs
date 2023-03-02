using Microsoft.AspNetCore.Mvc;

namespace WorkForYou.WebUI.Controllers;

public class ErrorController : Controller
{
    [HttpGet]
    public IActionResult PageNotFound()
    {
        return View();   
    }
    
    [HttpGet]
    public IActionResult AccessDenied() => View();
}
