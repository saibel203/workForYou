using WorkForYou.Core.Models;

namespace WorkForYou.Core.Responses.Repositories;

public class TypesOfCompanyResponse : BaseResponse
{
    public IEnumerable<TypesOfCompany> TypesOfCompanies { get; set; } = new List<TypesOfCompany>();
}
