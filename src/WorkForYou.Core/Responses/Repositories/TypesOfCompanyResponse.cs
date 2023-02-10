using WorkForYou.Core.Models;

namespace WorkForYou.Core.Responses.Repositories;

public class TypesOfCompanyResponse : BaseResponse
{
    public IReadOnlyList<TypesOfCompany> TypesOfCompanies { get; set; } = new List<TypesOfCompany>();
}
