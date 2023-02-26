using WorkForYou.Core.AdditionalModels;
using WorkForYou.Core.Models;
using WorkForYou.Core.Models.IdentityInheritance;

namespace WorkForYou.Data.Helpers;

public static class FilteringHelper
{
    public static IReadOnlyList<Vacancy> VacancyFiltering(QueryParameters queryParameters,
        IReadOnlyList<Vacancy> vacancies)
    {
        if (queryParameters.WorkCategory?.Length > 0)
            vacancies = vacancies
                .Where(vacancyData => queryParameters.WorkCategory
                    .Any(workCategory => workCategory == vacancyData.WorkCategory?.CategoryName)).ToList();

        if (queryParameters.EnglishLevel?.Length > 0)
            vacancies = vacancies
                .Where(vacancyData => queryParameters.EnglishLevel
                    .Any(englishLevel => englishLevel == vacancyData.EnglishLevel?.NameLevel)).ToList();

        if (queryParameters.CompanyType?.Length > 0)
            vacancies = vacancies
                .Where(vacancyData => queryParameters.CompanyType
                    .Any(companyType => companyType == vacancyData.TypeOfCompany?.TypeOfCompanyName)).ToList();

        if (queryParameters.HowToWork?.Length > 0)
            vacancies = vacancies
                .Where(vacancyData => queryParameters.HowToWork
                    .Any(howToWork => howToWork == vacancyData.HowToWork?.HowToWorkName)).ToList();

        if (queryParameters.CandidateRegion?.Length > 0)
            vacancies = vacancies
                .Where(vacancyData => queryParameters.CandidateRegion
                    .Any(candidateRegion => candidateRegion == vacancyData.CandidateRegion?.CandidateRegionName))
                .ToList();

        return vacancies;
    }

    public static IReadOnlyList<ApplicationUser> CandidateFiltering(QueryParameters queryParameters,
        IReadOnlyList<ApplicationUser> users)
    {
        if (queryParameters.WorkCategory?.Length > 0)
            users = users
                .Where(userData => queryParameters.WorkCategory
                    .Any(workCategory => workCategory == userData.CandidateUser?.CategoryWork?.CategoryName))
                .ToList();

        if (queryParameters.EnglishLevel?.Length > 0)
            users = users
                .Where(userData => queryParameters.EnglishLevel
                    .Any(englishLevel => englishLevel == userData.CandidateUser?.LevelEnglish?.NameLevel)).ToList();

        if (queryParameters.CommunicationLanguages?.Length > 0)
            users = users
                .Where(userData => queryParameters.CommunicationLanguages
                    .Any(communicationLanguage => communicationLanguage ==
                                                  userData.CandidateUser?.CommunicationLanguage
                                                      ?.CommunicationLanguageName)).ToList();
        return users;
    }

    public static IQueryable<Vacancy> RespondedListVacanciesFiltering(QueryParameters queryParameters,
        IQueryable<Vacancy> vacancies)
    {
        if (queryParameters.WorkCategory?.Length > 0)
            vacancies = vacancies
                .Where(vacancyData => queryParameters.WorkCategory
                    .Any(workCategory => workCategory == vacancyData.WorkCategory!.CategoryName));

        if (queryParameters.EnglishLevel?.Length > 0)
            vacancies = vacancies
                .Where(vacancyData => queryParameters.EnglishLevel
                    .Any(englishLevel => englishLevel == vacancyData.EnglishLevel!.NameLevel));

        if (queryParameters.CompanyType?.Length > 0)
            vacancies = vacancies
                .Where(vacancyData => queryParameters.CompanyType
                    .Any(companyType => companyType == vacancyData.TypeOfCompany!.TypeOfCompanyName));

        if (queryParameters.HowToWork?.Length > 0)
            vacancies = vacancies
                .Where(vacancyData => queryParameters.HowToWork
                    .Any(howToWork => howToWork == vacancyData.HowToWork!.HowToWorkName));

        if (queryParameters.CandidateRegion?.Length > 0)
            vacancies = vacancies
                .Where(vacancyData => queryParameters.CandidateRegion
                    .Any(candidateRegion => candidateRegion == vacancyData.CandidateRegion!.CandidateRegionName));

        return vacancies;
    }

    public static IQueryable<CandidateUser> FavouriteCandidateFiltering(QueryParameters queryParameters,
        IQueryable<CandidateUser> users)
    {
        if (queryParameters.WorkCategory?.Length > 0)
            users = users
                .Where(userData => queryParameters.WorkCategory
                    .Any(workCategory => workCategory == userData.CategoryWork!.CategoryName));

        if (queryParameters.EnglishLevel?.Length > 0)
            users = users
                .Where(userData => queryParameters.EnglishLevel
                    .Any(englishLevel => englishLevel == userData.LevelEnglish!.NameLevel));

        if (queryParameters.CommunicationLanguages?.Length > 0)
            users = users
                .Where(userData => queryParameters.CommunicationLanguages
                    .Any(communicationLanguage => communicationLanguage ==
                                                  userData.CommunicationLanguage
                                                      !.CommunicationLanguageName));
        return users;
    }

    public static IQueryable<ApplicationUser> ResponsesListCandidateFiltering(QueryParameters queryParameters,
        IQueryable<ApplicationUser> users)
    {
        if (queryParameters.WorkCategory?.Length > 0)
            users = users
                .Where(userData => queryParameters.WorkCategory
                    .Any(workCategory => workCategory == userData.CandidateUser!.CategoryWork!.CategoryName));

        if (queryParameters.EnglishLevel?.Length > 0)
            users = users
                .Where(userData => queryParameters.EnglishLevel
                    .Any(englishLevel => englishLevel == userData.CandidateUser!.LevelEnglish!.NameLevel));

        if (queryParameters.CommunicationLanguages?.Length > 0)
            users = users
                .Where(userData => queryParameters.CommunicationLanguages
                    .Any(communicationLanguage => communicationLanguage ==
                                                  userData.CandidateUser!.CommunicationLanguage
                                                      !.CommunicationLanguageName));
        
        return users;
    }
}