using WorkForYou.Core.Models;

namespace WorkForYou.Core.Responses.Repositories;

public class VacancyDomainResponse : BaseResponse
{
    public IEnumerable<VacancyDomain>? VacancyDomains { get; set; }
}
