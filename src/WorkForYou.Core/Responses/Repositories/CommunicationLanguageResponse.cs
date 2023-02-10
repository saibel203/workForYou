using WorkForYou.Core.Models;

namespace WorkForYou.Core.Responses.Repositories;

public class CommunicationLanguageResponse : BaseResponse
{
    public IReadOnlyList<CommunicationLanguage> CommunicationLanguages { get; set; } =
        new List<CommunicationLanguage>();
}
