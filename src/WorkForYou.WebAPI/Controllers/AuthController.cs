using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WorkForYou.Core.AdditionalModels;
using WorkForYou.Core.DTOModels.UserDTOs;
using WorkForYou.Core.Responses.Services;
using WorkForYou.Core.ServiceInterfaces;
using WorkForYou.Shared.OutputModels;
using WorkForYou.Shared.ViewModels.Forms;

namespace WorkForYou.WebAPI.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IApiAuthService _apiAuthService;
    private readonly IMapper _mapper;

    public AuthController(IMapper mapper, IApiAuthService apiAuthService)
    {
        _apiAuthService = apiAuthService;
        _mapper = mapper;
    }

    [HttpPost("login")] // /api/auth/login
    public async Task<IActionResult> Login([FromBody] LoginViewModel loginViewModel)
    {
        UserLoginDto userLoginDto = _mapper.Map<UserLoginDto>(loginViewModel);
        ApiError error = new ApiError();
        
        UserAuthResponse getAccessTokenResult = await _apiAuthService
            .GetUserAccessTokenAsync(userLoginDto);

        if (!getAccessTokenResult.IsSuccessfully)
        {
            error.ErrorCode = BadRequest().StatusCode;
            error.ErrorMessage = getAccessTokenResult.Message;
            return BadRequest(error);
        }

        var loginResponse = _mapper.Map<UserLoginOutput>(getAccessTokenResult);
        
        return Ok(loginResponse);
    }
}
