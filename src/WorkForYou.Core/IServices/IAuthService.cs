﻿using WorkForYou.Data.DtoModels;
using WorkForYou.Shared.Responses.Services;

namespace WorkForYou.Core.IServices;

public interface IAuthService
{
    Task<UserAuthResponse> RegisterAsync(UserRegisterDto? userRegisterDto);
    Task<UserAuthResponse> LoginAsync(UserLoginDto? userLoginDto);
    Task<UserAuthResponse> ConfirmEmailAsync(string userId, string token);
    Task<UserAuthResponse> ForgetPasswordAsync(ForgetPasswordDto? forgetPasswordDto);
    Task<UserAuthResponse> ResetPasswordAsync(ResetPasswordDto? resetPasswordDto);
    Task<UserAuthResponse> ChangePasswordAsync(ChangePasswordDto? changePasswordDto);
    Task LogoutAsync();
}