using Microsoft.AspNetCore.Mvc;
using WorkForYou.Core.AdditionalModels;
using WorkForYou.Core.DTOModels.UserDTOs;
using WorkForYou.Core.DTOModels.VacancyDTOs;
using WorkForYou.Core.Responses.Services;
using WorkForYou.Core.ServiceInterfaces;
using WorkForYou.WebAPI.Attributes;

namespace WorkForYou.WebAPI.Controllers;

public class FavouriteController : BaseController
{
    private readonly IFavouriteListService _favouriteListService;

    public FavouriteController(IFavouriteListService favouriteListService)
    {
        _favouriteListService = favouriteListService;
    }

    [HttpPost("newVacancy")] // /api/favourite/newVacancy
    [JwtAuthorize(Roles = ApplicationRoles.CandidateRole)]
    public async Task<IActionResult> VacancyToFavouriteCandidateList(VacancyOptionsDto vacancyOptionsDto)
    {
        string username = GetUsername();
        string userRole = GetUserRole();
        
        UsernameDto usernameDto = new UsernameDto {Username = username, UserRole = userRole};
        ApiError error = new ApiError();

        FavouriteListResponse addToFavouriteListResponse = await _favouriteListService
            .AddVacancyToFavouriteListAsync(usernameDto, vacancyOptionsDto.Id);

        if (!addToFavouriteListResponse.IsSuccessfully)
        {
            error.ErrorCode = BadRequest().StatusCode;
            error.ErrorMessage = addToFavouriteListResponse.Message;
            return BadRequest(error);
        }
        
        return Ok(new
        {
            LocalMessage = addToFavouriteListResponse.Message
        });
    }

    [HttpPost("newCandidate")] // /api/favourite/newCandidate
    [JwtAuthorize(Roles = ApplicationRoles.EmployerRole)]
    public async Task<IActionResult> CandidateToFavouriteEmployerList(VacancyOptionsDto vacancyOptionsDto)
    {
        string username = GetUsername();
        string userRole = GetUserRole();
        
        UsernameDto usernameDto = new UsernameDto {Username = username, UserRole = userRole};
        ApiError error = new ApiError();

        FavouriteListResponse addToFavouriteListResponse = await _favouriteListService
            .AddCandidateToFavouriteListAsync(usernameDto, vacancyOptionsDto.Id);

        if (!addToFavouriteListResponse.IsSuccessfully)
        {
            error.ErrorCode = BadRequest().StatusCode;
            error.ErrorMessage = addToFavouriteListResponse.Message;
            return BadRequest(error);
        }

        return Ok(new
        {
            LocalMessage = addToFavouriteListResponse.Message
        });
    }
}
