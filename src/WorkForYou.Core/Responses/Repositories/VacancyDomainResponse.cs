using WorkForYou.Core.Models;

namespace WorkForYou.Core.Responses.Repositories;

public class VacancyDomainResponse : BaseResponse
{
    public IReadOnlyList<VacancyDomain>? VacancyDomains { get; set; }
}
