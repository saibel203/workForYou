using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using WorkForYou.Core.ServiceInterfaces;
using WorkForYou.WebUI.Attributes;
using WorkForYou.WebUI.ViewModels.Forms;

namespace WorkForYou.WebUI.Controllers;

[Unauthorized]
public class HomeController : Controller
{
    private readonly IMailService _mailService;

    public HomeController(IMailService mailService)
    {
        _mailService = mailService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(ContactUsViewModel contactUsViewModel)
    {
        if (!ModelState.IsValid)
            return View(contactUsViewModel);

        await _mailService.SendToAdminEmailAsync("WorkForYou зворотній зв'язок " + contactUsViewModel.Subject,
            contactUsViewModel.Message);

        return View();
    }

    [HttpPost]
    public IActionResult SetCulture(string culture, string returnUrl)
    {
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions {Expires = DateTimeOffset.UtcNow.AddDays(30)}
        );
        return LocalRedirect(returnUrl);
    }
}