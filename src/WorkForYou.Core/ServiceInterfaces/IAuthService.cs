using WorkForYou.Core.DTOModels.UserDTOs;
using WorkForYou.Core.Responses.Services;

namespace WorkForYou.Core.ServiceInterfaces;

public interface IAuthService
{
    Task<UserAuthResponse> RegisterAsync(UserRegisterDto? userRegisterDto);
    Task<UserAuthResponse> LoginAsync(UserLoginDto? userLoginDto);
    Task<UserAuthResponse> ConfirmEmailAsync(string userId, string token);
    Task<UserAuthResponse> RemindPasswordAsync(RemindPasswordDto? forgetPasswordDto);
    Task<UserAuthResponse> ResetPasswordAsync(ResetPasswordDto? resetPasswordDto);
    Task<UserAuthResponse> ChangePasswordAsync(ChangePasswordDto? changePasswordDto);
    Task<UserAuthResponse> IsUserCandidate(UsernameDto? usernameDto);
    Task LogoutAsync();
}