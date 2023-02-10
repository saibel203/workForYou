using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using WorkForYou.Core.IOptions;
using WorkForYou.Core.IServices;
using WorkForYou.Core.Models.IdentityInheritance;
using WorkForYou.Core.DTOModels.UserDTOs;
using WorkForYou.Data.DatabaseContext;
using WorkForYou.Core.Responses.Services;

namespace WorkForYou.Services;

public class AuthService : IAuthService
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly WorkForYouDbContext _context;
    private readonly WebUiOptions _webUiOptions;
    private readonly IMailService _mailService;
    private readonly IMapper _mapper;

    public AuthService(UserManager<ApplicationUser> userManager, IMapper mapper, WorkForYouDbContext context, SignInManager<ApplicationUser> signInManager, IMailService mailService, IOptions<WebUiOptions> webUiOptions)
    {
        _userManager = userManager;
        _mapper = mapper;
        _context = context;
        _signInManager = signInManager;
        _mailService = mailService;
        _webUiOptions = webUiOptions.Value;
    }

    public async Task<UserAuthResponse> RegisterAsync(UserRegisterDto? userRegisterDto)
    {
        if (userRegisterDto is null)
            return new()
            {
                Message = "Register model error",
                IsSuccessfully = false
            };

        var user = _mapper.Map<ApplicationUser>(userRegisterDto);

        var createUserResult = await _userManager.CreateAsync(user, userRegisterDto.Password);

        if (!createUserResult.Succeeded)
            return new()
            {
                Message = "Create user error",
                IsSuccessfully = false,
                Errors = createUserResult.Errors
            };

        await _userManager.AddToRoleAsync(user, userRegisterDto.SelectedRole);

        var confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var encodedEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);
        var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);

        string url = $"{_webUiOptions.ApplicationUrl}/auth/confirmEmailResult" +
                     $"?userId={user.Id}&token={validEmailToken}";

        // await _mailService.SendEmailAsync(user.Email!, "Confirm email", $"For confirm, <a href='{url}' " +
        //                                                                 $"target='_blank'>CLICK</a>");

        await _mailService.SendEmailAsync(user.Email!, "Підтвердження email", 
            "Підтвердження email на сайті WorkForYou. Якщо ви не реєструвались, проігноруйте цей лист.", 
            url, "Натисніть для підтвердження");
        
        if (userRegisterDto.SelectedRole == "employer")
        {
            await _context.EmployerUsers.AddAsync(new ()
            {
                ApplicationUser = user
            });
            await _context.SaveChangesAsync();
        }
        else if (userRegisterDto.SelectedRole == "candidate")
        {
            await _context.CandidateUsers.AddAsync(new()
            {
                ApplicationUser = user
            });
            await _context.SaveChangesAsync();
        }
            

        return new()
        {
            Message = "User created successfully",
            IsSuccessfully = true
        };
    }

    public async Task<UserAuthResponse> LoginAsync(UserLoginDto? userLoginDto)
    {
        if (userLoginDto is null)
            return new()
            {
                Message = "Login model error",
                IsSuccessfully = false
            };

        var user = await _userManager.FindByEmailAsync(userLoginDto.Email);

        if (user is null)
            return new()
            {
                Message = "Користувача з таким Email не знайдено",
                IsSuccessfully = false
            };

        var loginUserResult = await _signInManager.PasswordSignInAsync(user, userLoginDto.Password, 
            userLoginDto.RememberMe, false);

        if (!loginUserResult.Succeeded)
        {
            if (!user.EmailConfirmed)
                return new()
                {
                    Message = "Спочатку підтвердіть електронну пошту",
                    IsSuccessfully = false
                };
            
            return new()
            {
                Message = "Не вірний пароль",
                IsSuccessfully = false
            };
            
        }

        return new()
        {
            Message = "Success",
            IsSuccessfully = true
        };
    }

    public async Task<UserAuthResponse> ConfirmEmailAsync(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return new()
            {
                Message = "Користувача з заданим ідентифікатором не знайдено",
                IsSuccessfully = false
            };

        var decodedToken = WebEncoders.Base64UrlDecode(token);
        var normalToken = Encoding.UTF8.GetString(decodedToken);

        var result = await _userManager.ConfirmEmailAsync(user, normalToken);

        if (!result.Succeeded)
            return new()
            {
                Message = "Помилка підтвердження",
                IsSuccessfully = false,
                Errors = result.Errors
            };

        return new()
        {
            Message = "",
            IsSuccessfully = true
        };
    }

    public async Task<UserAuthResponse> RemindPasswordAsync(RemindPasswordDto? forgetPasswordDto)
    {
        if (forgetPasswordDto is null)
            return new()
            {
                Message = "Помилка заповнення форми",
                IsSuccessfully = false
            };
        
        var user = await _userManager.FindByEmailAsync(forgetPasswordDto.Email);

        if (user is null)
            return new()
            {
                Message = "Користувача з таким Email не знайдено",
                IsSuccessfully = false
            };

        var defaultToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        var encodedToken = Encoding.UTF8.GetBytes(defaultToken);
        var currentToken = WebEncoders.Base64UrlEncode(encodedToken);

        var url = $"{_webUiOptions.ApplicationUrl}/Auth/ResetPassword?email={forgetPasswordDto.Email}&token={currentToken}";

        // await _mailService.SendEmailAsync(forgetPasswordDto.Email, "Reset password", "For reset password" +
        //                                                                $"<a href='{url}' target='_blank'>Click here</a>");

        await _mailService.SendEmailAsync(forgetPasswordDto.Email, "Відновлення паролю",
            "Відновлення паролю на сайті WorkForYou. Якщо ви нічого не змінювали, проігноруйте це повідомлення.",
            url, "Відновлення паролю");
        
        return new()
        {
            Message = "URL для відновлення пароля відправлено на вашу почту",
            IsSuccessfully = true
        };
    }

    public async Task<UserAuthResponse> ResetPasswordAsync(ResetPasswordDto? resetPasswordDto)
    {
        if (resetPasswordDto is null)
            return new()
            {
                Message = "Помилка заповнення форми",
                IsSuccessfully = false
            };
        
        var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);

        if (user is null)
            return new()
            {
                Message = "Користувача з заданим Email не знайдено",
                IsSuccessfully = false
            };

        if (resetPasswordDto.NewPassword != resetPasswordDto.ConfirmNewPassword)
            return new()
            {
                Message = "Паролі не співпадають",
                IsSuccessfully = false
            };

        var decodedToken = WebEncoders.Base64UrlDecode(resetPasswordDto.Token);
        var normalToken = Encoding.UTF8.GetString(decodedToken);

        var resetPasswordResult = await _userManager.ResetPasswordAsync(user, normalToken, resetPasswordDto.NewPassword);

        if (!resetPasswordResult.Succeeded)
            return new()
            {
                Message = "Присутні помилки",
                IsSuccessfully = false,
                Errors = resetPasswordResult.Errors
            };

        await _mailService.SendEmailAsync(resetPasswordDto.Email, "Змінено паролю", 
            $"На сайті WorkForYou ваш пароль було змінено на {resetPasswordDto.NewPassword}. Якщо ви нічого не змінювали, проігноруйте цей лист.",
            "", "Перейти на сайт");

        return new()
        {
            Message = "Пароль успішно змінено",
            IsSuccessfully = true
        };
    }

    public async Task<UserAuthResponse> ChangePasswordAsync(ChangePasswordDto? changePasswordDto)
    {
        if (changePasswordDto is null)
            return new()
            {
                Message = "Помилка заповнення форми",
                IsSuccessfully = false
            };

        var currentUser = await _userManager.FindByEmailAsync(changePasswordDto.Email);

        if (currentUser is null)
            return new()
            {
                Message = "Користувача з таким Email не знайдено",
                IsSuccessfully = false
            };

        if (changePasswordDto.NewPassword != changePasswordDto.ConfirmNewPassword)
            return new()
            {
                Message = "Паролі не співпадають",
                IsSuccessfully = false
            };
        
        var changePasswordResult = await _userManager.ChangePasswordAsync(currentUser, changePasswordDto.OldPassword, changePasswordDto.NewPassword);

        if (!changePasswordResult.Succeeded)
            return new()
            {
                Message = "Помилка зміни пароля",
                IsSuccessfully = false,
                Errors = changePasswordResult.Errors
            };

        await _mailService.SendEmailAsync(changePasswordDto.Email, "Пароль змінено", 
            $"На сайті WorkForYou ваш пароль було змінено на {changePasswordDto.NewPassword}. Якщо ви нічого не змінювали, проігноруйте цей лист.", 
            "", "");

        return new()
        {
            Message = "Парль успішно змінений",
            IsSuccessfully = true
        };
    }

    public async Task LogoutAsync() { await _signInManager.SignOutAsync(); }

    public async Task<UserAuthResponse> IsUserCandidate(UsernameDto? usernameDto)
    {
        if (usernameDto is null)
            return new()
            {
                Message = "Error getting data",
                IsSuccessfully = false
            };

        var userDataResult = await _userManager.FindByNameAsync(usernameDto.Username);

        if (userDataResult is null)
            return new()
            {
                Message = "The user with the given Username was not found",
                IsSuccessfully = false
            };

        const string candidateRole = "candidate";

        var isUserCandidate = await _userManager.IsInRoleAsync(userDataResult, candidateRole);

        if (isUserCandidate)
            return new()
            {
                Message = "The user is a candidate",
                IsSuccessfully = true,
                IsUserCandidate = true
            };

        return new()
        {
            Message = "The user is an employer",
            IsSuccessfully = true,
            IsUserCandidate = false
        };
    }
}
