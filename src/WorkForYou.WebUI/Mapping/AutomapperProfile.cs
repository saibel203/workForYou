﻿using AutoMapper;
using WorkForYou.Core.Models.IdentityInheritance;
using WorkForYou.Core.DTOModels.UserDTOs;
using WorkForYou.Core.DTOModels.VacancyDTOs;
using WorkForYou.Core.Models;
using WorkForYou.WebUI.ViewModels.Forms;

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
        
        CreateMap<ActionVacancyViewModel, ActionVacancyDto>().ReverseMap();
        CreateMap<ActionVacancyDto, Vacancy>().ReverseMap();
        CreateMap<Vacancy, ActionVacancyViewModel>().ReverseMap();

        CreateMap<ApplicationUser, RefreshGeneralProfileInfoViewModel>();
        CreateMap<RefreshGeneralProfileInfoViewModel, RefreshGeneralUserDto>();

        CreateMap<CandidateUser, RefreshCandidateInfoViewModel>();
        CreateMap<RefreshCandidateDto, CandidateUser>();
        CreateMap<RefreshCandidateInfoViewModel, RefreshCandidateDto>();

        CreateMap<RefreshEmployerDto, EmployerUser>();
        CreateMap<RefreshEmployerInfoViewModel, RefreshEmployerDto>();
        CreateMap<EmployerUser, RefreshEmployerInfoViewModel>();
    }
}