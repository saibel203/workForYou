using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkForYou.Core.IRepositories;
using WorkForYou.WebUI.ViewModels;

namespace WorkForYou.WebUI.Controllers;

[Authorize(Roles = "candidate")]
public class CandidateAccountController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public CandidateAccountController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<IActionResult> AllVacancies(int pageNumber, string searchString)
    {
        if (pageNumber < 1)
            return RedirectToAction("AllVacancies", new {pageNumber = 1});

        var vacancies = await _unitOfWork.VacancyRepository.GetAllVacanciesAsync(pageNumber, searchString);

        if (pageNumber > vacancies.PageCount)
            return RedirectToAction("AllVacancies", new {pageNumber = vacancies.PageCount});

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

        return View(vacanciesViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> FavouriteList()
    {
        var favouriteVacancies =
            await _unitOfWork.UserRepository.ShowFavouriteListAsync(new() {Username = User.Identity?.Name!});

        return View(favouriteVacancies.FavouriteList);
    }

    [HttpGet]
    public async Task<IActionResult> AddToFavouriteList(int id)
    {
        await _unitOfWork.VacancyRepository.AddVacancyToFavouriteList(User.Identity!.Name!, id);

        return RedirectToAction("AllVacancies");
    }
}