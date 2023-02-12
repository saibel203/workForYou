using WorkForYou.Core.Models;
using WorkForYou.Core.Models.IdentityInheritance;

namespace WorkForYou.Data.Helpers;

public static class ListSortingHelper
{
    private const string PublicationDataSort = "publication-date";
    private const string FromSalarySort = "from-salary";
    private const string ToSalarySort = "to-salary";
    private const string FromExperienceSort = "from-experience";
    private const string ToExperienceSort = "to-experience";
    private const string FromViewCountSort = "from-view-count";
    private const string ToViewCountSort = "to-view-count";
    
    public static IReadOnlyList<Vacancy> ListVacancySort(string sortBy, IReadOnlyList<Vacancy> vacancies)
    {
        vacancies = sortBy switch
        {
            PublicationDataSort => vacancies.OrderBy(x => x.CreatedDate).ToList(),
            FromSalarySort => vacancies.OrderByDescending(x => x.ToSalary).ToList(),
            ToSalarySort => vacancies.OrderBy(x => x.FromSalary).ToList(),
            FromExperienceSort => vacancies.OrderByDescending(x => x.ExperienceWork).ToList(),
            ToExperienceSort => vacancies.OrderBy(x => x.ExperienceWork).ToList(),
            FromViewCountSort => vacancies.OrderByDescending(x => x.ViewCount).ToList(),
            ToViewCountSort => vacancies.OrderBy(x => x.ViewCount).ToList(),
            _ => vacancies.OrderByDescending(x => x.CreatedDate).ToList()
        };
        
        return vacancies;
    }

    public static IReadOnlyList<ApplicationUser> ListUserSort(string sortBy, IReadOnlyList<ApplicationUser> users)
    {
        users = sortBy switch
        {
            PublicationDataSort => users.OrderByDescending(x => x.CandidateUser!.CreatedDate).ToList(),
            FromSalarySort => users.OrderBy(x => x.CandidateUser!.ExpectedSalary).ToList(),
            ToSalarySort => users.OrderByDescending(x => x.CandidateUser!.ExpectedSalary).ToList(),
            FromExperienceSort => users.OrderBy(x => x.CandidateUser!.ExperienceWorkTime).ToList(),
            ToExperienceSort => users.OrderByDescending(x => x.CandidateUser!.ExperienceWorkTime).ToList(),
            FromViewCountSort => users.OrderBy(x => x.CandidateUser!.ViewCount).ToList(),
            ToViewCountSort => users.OrderByDescending(x => x.CandidateUser!.ViewCount).ToList(),
            _ => users.OrderBy(x => x.CandidateUser!.CreatedDate).ToList()
        };
        
        return users;
    }

    public static IQueryable<Vacancy> FavouriteListVacancySort(string sortBy, IQueryable<Vacancy> vacancies)
    {
        vacancies = sortBy switch
        {
            PublicationDataSort => vacancies.OrderBy(x => x.CreatedDate),
            FromSalarySort => vacancies.OrderByDescending(x => x.ToSalary),
            ToSalarySort => vacancies.OrderBy(x => x.FromSalary),
            FromExperienceSort => vacancies.OrderByDescending(x => x.ExperienceWork),
            ToExperienceSort => vacancies.OrderBy(x => x.ExperienceWork),
            FromViewCountSort => vacancies.OrderByDescending(x => x.ViewCount),
            ToViewCountSort => vacancies.OrderBy(x => x.ViewCount),
            _ => vacancies.OrderByDescending(x => x.CreatedDate)
        };
        
        return vacancies;
    }

    public static IQueryable<CandidateUser> FavouriteListUserSort(string sortBy, IQueryable<CandidateUser> users)
    {
        users = sortBy switch
        {
            PublicationDataSort => users.OrderByDescending(x => x.CreatedDate),
            FromSalarySort => users.OrderBy(x => x.ExpectedSalary),
            ToSalarySort => users.OrderByDescending(x => x.ExpectedSalary),
            FromExperienceSort => users.OrderBy(x => x.ExperienceWorkTime),
            ToExperienceSort => users.OrderByDescending(x => x.ExperienceWorkTime),
            FromViewCountSort => users.OrderBy(x => x.ViewCount),
            ToViewCountSort => users.OrderByDescending(x => x.ViewCount),
            _ => users.OrderBy(x => x.CreatedDate)
        };

        return users;
    }
}
