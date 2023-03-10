using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WorkForYou.Core.DTOModels.UserDTOs;
using WorkForYou.Core.IOptions;
using WorkForYou.Core.Models.IdentityInheritance;
using WorkForYou.Core.Responses.Services;
using WorkForYou.Core.ServiceInterfaces;

namespace WorkForYou.Services;

public class ApiAuthService : IApiAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<ApiAuthService> _logger;
    private readonly JwtOptions _jwtOptions;

    public ApiAuthService(UserManager<ApplicationUser> userManager, IOptions<JwtOptions> jwtOptions, ILogger<ApiAuthService> logger)
    {
        _userManager = userManager;
        _logger = logger;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<UserAuthResponse> GetUserAccessTokenAsync(UserLoginDto? userLoginDto)
    {
        try
        {
            if (userLoginDto is null)
                return new()
                {
                    Message = "Error getting user",
                    IsSuccessfully = false
                };

            var userData = await _userManager.FindByEmailAsync(userLoginDto.Email);

            if (userData is null)
                return new()
                {
                    Message = "User with this Email was not found",
                    IsSuccessfully = false
                };

            var checkedUserPassword = await _userManager.CheckPasswordAsync(userData, userLoginDto.Password);
            
            if (!checkedUserPassword)
                return new()
                {
                    Message = "Invalid password",
                    IsSuccessfully = false
                };

            var userRoles = await _userManager.GetRolesAsync(userData);
            var userRole = userRoles[0];

            var userClaims = new List<Claim>
            {
                new(ClaimTypes.Role, userRole),
                new(ClaimTypes.NameIdentifier, userData.Id),
                new(ClaimTypes.Name, userData.UserName!),
                new(ClaimTypes.Email, userData.Email!)
            };

            var encodingKey = Encoding.UTF8.GetBytes(_jwtOptions.Key);

            var authSigningKey = new SymmetricSecurityKey(encodingKey);

            var tokenResult = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: userClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

            var token = new JwtSecurityTokenHandler().WriteToken(tokenResult);

            return new()
            {
                Message = "Authorization was successful",
                IsSuccessfully = true,
                Token = token,
                Expires = DateTime.Now.AddDays(1)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during authorization");
            return new()
            {
                Message = "An error occurred during authorization",
                IsSuccessfully = false
            };
        }
    }
}
