using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkForYou.Core.DtoModels;
using WorkForYou.Core.IRepositories;
using WorkForYou.WebUI.ViewModels;

namespace WorkForYou.WebUI.Controllers;

[Authorize(Roles = "employer")]
public class EmployerAccountController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public EmployerAccountController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IActionResult> AllVacancies(int pageNumber, string searchString)
    {
        var usernameDto = new UsernameDto {Username = User.Identity?.Name!};
        var vacancies =
            await _unitOfWork.VacancyRepository.GetAllEmployerVacanciesAsync(usernameDto, pageNumber, searchString);

        var vacanciesViewModel = new VacanciesViewModel
        {
            CurrentController = ControllerContext.ActionDescriptor.ControllerName,
            PageNumber = pageNumber,
            PageCount = vacancies.PageCount,
            VacancyCount = vacancies.VacancyCount,
            Vacancies = vacancies.VacancyList,
            SearchString = searchString,
            Pages = vacancies.Pages
        };

        if (pageNumber < 1)
        {
            if (vacancies.VacancyCount == 0)
                return View(vacanciesViewModel);

            return RedirectToAction("AllVacancies", new {pageNumber = 1});
        }

        if (pageNumber > vacancies.PageCount)
            return RedirectToAction("AllVacancies", new {pageNumber = vacancies.PageCount});

        return View(vacanciesViewModel);
    }
}