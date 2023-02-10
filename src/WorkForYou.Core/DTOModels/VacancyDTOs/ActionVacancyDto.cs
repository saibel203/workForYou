using WorkForYou.Core.Models;

namespace WorkForYou.Core.DTOModels.VacancyDTOs;

public class ActionVacancyDto
{
    public int VacancyId { get; set; }
    public string VacancyTitle { get; set; } = string.Empty;
    public int VacancyDomainId { get; set; }
    public string ShortDescription { get; set; } = string.Empty;
    public string LongDescription { get; set; } = string.Empty;
    public string KeyWords { get; set; } = string.Empty;
    public int WorkCategoryId { get; set; }
    public int HowToWorkId { get; set; }
    public int RelocateId { get; set; }
    public int CandidateRegionId { get; set; }
    public int FromSalary { get; set; }
    public int ToSalary { get; set; }
    public int ExperienceWork { get; set; }
    public int EnglishLevelId { get; set; }
    public int TypesOfCompanyId { get; set; }
    public EmployerUser? EmployerUser { get; set; }
}
