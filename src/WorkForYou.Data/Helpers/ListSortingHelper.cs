using WorkForYou.Core.AdditionalModels;
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
    private const string FromRespondCountSort = "from-respond-count";
    private const string ToRespondCountSort = "to-respond-count";

    public static IReadOnlyList<Vacancy> ListVacancySort(QueryParameters queryParameters,
        IReadOnlyList<Vacancy> vacancies)
    {
        if (!string.IsNullOrEmpty(queryParameters.SortBy))
            vacancies = queryParameters.SortBy switch
            {
                PublicationDataSort => vacancies.OrderBy(x => x.CreatedDate).ToList(),
                FromSalarySort => vacancies.OrderByDescending(x => x.ToSalary).ToList(),
                ToSalarySort => vacancies.OrderBy(x => x.FromSalary).ToList(),
                FromExperienceSort => vacancies.OrderByDescending(x => x.ExperienceWork).ToList(),
                ToExperienceSort => vacancies.OrderBy(x => x.ExperienceWork).ToList(),
                FromViewCountSort => vacancies.OrderByDescending(x => x.ViewCount).ToList(),
                ToViewCountSort => vacancies.OrderBy(x => x.ViewCount).ToList(),
                FromRespondCountSort => vacancies.OrderByDescending(x => x.ReviewsCount).ToList(),
                ToRespondCountSort => vacancies.OrderBy(x => x.ReviewsCount).ToList(),
                _ => vacancies.OrderByDescending(x => x.CreatedDate).ToList()
            };

        return vacancies;
    }

    public static IReadOnlyList<ApplicationUser> ListUserSort(QueryParameters queryParameters,
        IReadOnlyList<ApplicationUser> users)
    {
        if (!string.IsNullOrEmpty(queryParameters.SortBy))
            users = queryParameters.SortBy switch
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

    public static IQueryable<Vacancy> FavouriteListVacancySort(QueryParameters queryParameters,
        IQueryable<Vacancy> vacancies)
    {
        if (!string.IsNullOrEmpty(queryParameters.SortBy))
            vacancies = queryParameters.SortBy switch
            {
                PublicationDataSort => vacancies.OrderBy(x => x.CreatedDate),
                FromSalarySort => vacancies.OrderByDescending(x => x.ToSalary),
                ToSalarySort => vacancies.OrderBy(x => x.FromSalary),
                FromExperienceSort => vacancies.OrderByDescending(x => x.ExperienceWork),
                ToExperienceSort => vacancies.OrderBy(x => x.ExperienceWork),
                FromViewCountSort => vacancies.OrderByDescending(x => x.ViewCount),
                ToViewCountSort => vacancies.OrderBy(x => x.ViewCount),
                FromRespondCountSort => vacancies.OrderByDescending(x => x.ReviewsCount),
                ToRespondCountSort => vacancies.OrderBy(x => x.ReviewsCount),
                _ => vacancies.OrderByDescending(x => x.CreatedDate)
            };

        return vacancies;
    }

    public static IQueryable<CandidateUser> FavouriteListUserSort(QueryParameters queryParameters,
        IQueryable<CandidateUser> users)
    {
        if (!string.IsNullOrEmpty(queryParameters.SortBy))
            users = queryParameters.SortBy switch
            {
                PublicationDataSort => users.OrderBy(x => x.CreatedDate),
                FromSalarySort => users.OrderByDescending(x => x.ExpectedSalary),
                ToSalarySort => users.OrderBy(x => x.ExpectedSalary),
                FromExperienceSort => users.OrderByDescending(x => x.ExperienceWorkTime),
                ToExperienceSort => users.OrderBy(x => x.ExperienceWorkTime),
                FromViewCountSort => users.OrderByDescending(x => x.ViewCount),
                ToViewCountSort => users.OrderBy(x => x.ViewCount),
                _ => users.OrderByDescending(x => x.CreatedDate)
            };

        return users;
    }

    public static IQueryable<ApplicationUser> VacancyResponsesListSort(QueryParameters queryParameters,
        IQueryable<ApplicationUser> users)
    {
        if (!string.IsNullOrEmpty(queryParameters.SortBy))
            users = queryParameters.SortBy switch
            {
                PublicationDataSort => users.OrderBy(x => x.CandidateUser!.CreatedDate),
                FromSalarySort => users.OrderByDescending(x => x.CandidateUser!.ExpectedSalary),
                ToSalarySort => users.OrderBy(x => x.CandidateUser!.ExpectedSalary),
                FromExperienceSort => users.OrderByDescending(x => x.CandidateUser!.ExperienceWorkTime),
                ToExperienceSort => users.OrderBy(x => x.CandidateUser!.ExperienceWorkTime),
                FromViewCountSort => users.OrderByDescending(x => x.CandidateUser!.ViewCount),
                ToViewCountSort => users.OrderBy(x => x.CandidateUser!.ViewCount),
                _ => users.OrderByDescending(x => x.CandidateUser!.CreatedDate)
            };

        return users;
    }
}