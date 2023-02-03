using AutoMapper;
using WorkForYou.Core.Models.IdentityInheritance;
using WorkForYou.Core.DtoModels;
using WorkForYou.Core.Models;
using WorkForYou.WebUI.ViewModels;

namespace WorkForYou.WebUI.Mapping;

public class AutomapperProfile : Profile
{
    public AutomapperProfile()
    {
        CreateMap<UserRegisterDto, ApplicationUser>();
        CreateMap<RegisterViewModel, UserRegisterDto>();
        CreateMap<LoginViewModel, UserLoginDto>();
        CreateMap<ResetPasswordViewModel, ResetPasswordDto>();
        CreateMap<RemindPasswordViewModel, RemindPasswordDto>();
        CreateMap<ChangePasswordViewModel, ChangePasswordDto>();
        CreateMap<ChangePasswordViewModel, UsernameDto>();
        
        CreateMap<ActionVacancyDto, ActionVacancyViewModel>().ReverseMap();
        CreateMap<ActionVacancyDto, Vacancy>().ReverseMap();
        CreateMap<Vacancy, ActionVacancyViewModel>().ReverseMap();
        CreateMap<CreateVacancyViewModel, ActionVacancyDto>();
        CreateMap<UpdateVacancyViewModel, ActionVacancyDto>();
    }
}