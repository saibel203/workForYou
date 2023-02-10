namespace WorkForYou.Core.Responses;

public class ListBaseResponse : BaseResponse
{
    public int PageCount { get; set; }
    public int VacancyCount { get; set; }
    public string? SearchString { get; set; } = string.Empty;
    public string? SortBy { get; set; } = string.Empty;
    public IEnumerable<int> Pages { get; set; } = new List<int>();
}
