using AutoMapper;
using WorkForYou.Data.DtoModels;
using WorkForYou.Data.Models.IdentityInheritance;
using WorkForYou.WebUI.ViewModels;

namespace WorkForYou.WebUI.Mapping;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<UserRegisterDto, ApplicationUser>();
        CreateMap<RegisterViewModel, UserRegisterDto>();
        CreateMap<LoginViewModel, UserLoginDto>();
        CreateMap<ResetPasswordViewModel, ResetPasswordDto>();
        CreateMap<ForgetPasswordViewModel, ForgetPasswordDto>();
        CreateMap<ChangePasswordViewModel, ChangePasswordDto>();
    }
}
