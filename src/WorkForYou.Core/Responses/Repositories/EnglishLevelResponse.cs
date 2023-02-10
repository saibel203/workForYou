using WorkForYou.Core.Models;

namespace WorkForYou.Core.Responses.Repositories;

public class EnglishLevelResponse : BaseResponse
{
    public IReadOnlyList<EnglishLevel> EnglishLevels { get; set; } = new List<EnglishLevel>();
}
