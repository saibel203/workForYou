using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WorkForYou.Core.AdditionalModels;
using WorkForYou.Core.DTOModels.UserDTOs;
using WorkForYou.Core.DTOModels.VacancyDTOs;
using WorkForYou.Core.RepositoryInterfaces;
using WorkForYou.Core.Responses.Repositories;
using WorkForYou.WebAPI.Attributes;

namespace WorkForYou.WebAPI.Controllers;

public class RespondedController : BaseController
{
    private readonly IStringLocalizer<RespondedController> _stringLocalization;
    private readonly IUnitOfWork _unitOfWork;

    public RespondedController(IUnitOfWork unitOfWork, IStringLocalizer<RespondedController> stringLocalization)
    {
        _unitOfWork = unitOfWork;
        _stringLocalization = stringLocalization;
    }

    [HttpPost("newResponded")] // /api/responded/newResponded
    [JwtAuthorize(Roles = ApplicationRoles.CandidateRole)]
    public async Task<IActionResult> NewCandidateToRespondedList(VacancyOptionsDto vacancyOptionsDto)
    {
        string username = GetUsername();
        string userRole = GetUserRole();

        UsernameDto usernameDto = new UsernameDto {Username = username, UserRole = userRole};
        ApiError error = new ApiError();
        
        RespondedListResponse respondedResult = await _unitOfWork.RespondedListRepository
            .RespondToVacancyAsync(usernameDto, vacancyOptionsDto.Id);

                if (!respondedResult.IsSuccessfully)
        {
            error.ErrorCode = BadRequest().StatusCode;
            error.ErrorMessage = respondedResult.Message;
            return BadRequest(error);
        }
        
        return Ok(new 
        {
            Message = _stringLocalization["SuccessRespond"]
        });
    }
    
    [HttpPost("removeResponded")] // /api/responded/removeResponded
    [JwtAuthorize(Roles = ApplicationRoles.CandidateRole)]
    public async Task<IActionResult> RemoveCandidateToRespondedList(VacancyOptionsDto vacancyOptionsDto)
    {
        string username = GetUsername();
        string userRole = GetUserRole();

        UsernameDto usernameDto = new UsernameDto {Username = username, UserRole = userRole};
        ApiError error = new ApiError();
        
        RespondedListResponse respondedResult = await _unitOfWork.RespondedListRepository
            .RemoveRespondToVacancyAsync(usernameDto, vacancyOptionsDto.Id);

        if (!respondedResult.IsSuccessfully)
        {
            error.ErrorCode = BadRequest().StatusCode;
            error.ErrorMessage = respondedResult.Message;
            return BadRequest(error);
        }

        return Ok(new
        {
            Message = _stringLocalization["SuccessCancelRespond"]
        });
    }
}
