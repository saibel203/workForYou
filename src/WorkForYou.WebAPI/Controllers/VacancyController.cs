using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkForYou.Core.AdditionalModels;
using WorkForYou.Core.RepositoryInterfaces;
using WorkForYou.Core.ServiceInterfaces;

namespace WorkForYou.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VacancyController : ControllerBase
{
    private readonly INotificationService _notificationService;
    private readonly IUnitOfWork _unitOfWork;
    
    public VacancyController(IUnitOfWork unitOfWork, INotificationService notificationService)
    {
        _unitOfWork = unitOfWork;
        _notificationService = notificationService;
    }
    
    [HttpDelete("remove/{vacancyId:int}")] // /api/vacancy/remove/{vacancyId}
    [Authorize(Roles = "employer")]
    public async Task<IActionResult> RemoveVacancy(int vacancyId)
    {
        var vacancyRemoveResult = await _unitOfWork.VacancyRepository.RemoveVacancyAsync(vacancyId);
        var apiError = new ApiError();
    
        if (!vacancyRemoveResult.IsSuccessfully)
        {
            //_notificationService.CustomErrorMessage("Помилка при спробі видалити вакансію");
            
            apiError.ErrorCode = BadRequest().StatusCode;
            apiError.ErrorMessage = vacancyRemoveResult.Message;
            return BadRequest(apiError);
        }
    
        //_notificationService.CustomSuccessMessage("Вакансія успішно видалена");
        return Ok();
    }
}
