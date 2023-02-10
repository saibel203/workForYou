namespace WorkForYou.Core.AdditionalModels;

public class QueryParameters
{
    public int PageNumber { get; set; }
    public string? SearchString { get; set; } = string.Empty;
    public string? SortBy { get; set; } = string.Empty;
    public string? Username { get; set; } = string.Empty;
}
