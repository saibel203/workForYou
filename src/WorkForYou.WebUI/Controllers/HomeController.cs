using Microsoft.AspNetCore.Mvc;
using WorkForYou.Core.IServices;
using WorkForYou.WebUI.Attributes;
using WorkForYou.WebUI.ViewModels;

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

        await _mailService.SendToAdminEmailAsync(contactUsViewModel.Email, "WorkForYou зворотній зв'язок " + contactUsViewModel.Subject,
            contactUsViewModel.Message);

        return View();
    }
}