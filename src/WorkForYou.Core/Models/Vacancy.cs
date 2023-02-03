namespace WorkForYou.Core.Models;

public class Vacancy
{
    public int VacancyId { get; set; }
    public string VacancyTitle { get; set; } = string.Empty;
    
    public int VacancyDomainId { get; set; }
    public VacancyDomain? VacancyDomain { get; set; }
    
    public string ShortDescription { get; set; } = string.Empty;
    public string LongDescription { get; set; } = string.Empty;

    public int WorkCategoryId { get; set; }
    public WorkCategory? WorkCategory { get; set; }

    // public IEnumerable<string>? Keywords { get; set; }

    public int HowToWorkId { get; set; }
    public HowToWork? HowToWork { get; set; }
    
    // public IEnumerable<string>? Country { get; set; }
    // public IEnumerable<string>? Cities { get; set; }

    public int RelocateId { get; set; }
    public Relocate? Relocate { get; set; }

    public int CandidateRegionId { get; set; }
    public CandidateRegion? CandidateRegion { get; set; }

    public int FromSalary { get; set; }
    public int ToSalary { get; set; }
    public int ExperienceWork { get; set; }

    public int EnglishLevelId { get; set; }
    public EnglishLevel? EnglishLevel { get; set; }

    public int TypesOfCompanyId { get; set; }
    public TypesOfCompany? TypeOfCompany { get; set; }

    public EmployerUser? EmployerUser { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public int ViewCount { get; set; }
    public ICollection<FavouriteVacancy>? FavouriteVacancyCollection { get; set; } = new List<FavouriteVacancy>();
}
