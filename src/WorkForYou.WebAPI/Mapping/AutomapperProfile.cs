using AutoMapper;
using WorkForYou.Core.DTOModels.UserDTOs;
using WorkForYou.Core.DTOModels.VacancyDTOs;
using WorkForYou.Core.Models;
using WorkForYou.Core.Models.IdentityInheritance;

namespace WorkForYou.WebAPI.Mapping;

public class AutomapperProfile : Profile
{
    public AutomapperProfile()
    {
        CreateMap<UserRegisterDto, ApplicationUser>();
        CreateMap<ActionVacancyDto, Vacancy>();
        CreateMap<RefreshCandidateDto, CandidateUser>();
        
        CreateMap<RefreshEmployerDto, EmployerUser>();
    }
}
