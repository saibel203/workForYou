namespace WorkForYou.Core.AdditionalModels;

public class QueryParameters
{
    public int PageNumber { get; set; }
    public string? SearchString { get; set; } = string.Empty;
    public string? SortBy { get; set; } = string.Empty;
    public string? Username { get; set; } = string.Empty;
    public string[]? WorkCategory { get; set; }
    public string[]? EnglishLevel { get; set; }
    public string[]? CompanyType { get; set; }
    public string[]? HowToWork { get; set; }
    public string[]? CandidateRegion { get; set; }
    public string[]? CommunicationLanguages { get; set; }
}
