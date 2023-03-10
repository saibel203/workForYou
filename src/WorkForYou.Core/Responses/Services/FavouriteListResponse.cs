using WorkForYou.Core.Models;

namespace WorkForYou.Core.Responses.Services;

public class FavouriteListResponse : ListBaseResponse
{
    public IEnumerable<Vacancy> FavouriteVacancyList { get; set; } = new List<Vacancy>();
    public IEnumerable<CandidateUser> FavouriteCandidateList { get; set; } = new List<CandidateUser>();
    public bool IsVacancyInFavouriteList { get; set; }
    public bool IsCandidateInFavouriteList { get; set; }
}
