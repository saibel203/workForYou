namespace WorkForYou.Core.Models;

public class CommunicationLanguage
{
    public int CommunicationLanguageId { get; set; }
    public string? CommunicationLanguageName { get; set; } = string.Empty;
    public IEnumerable<CandidateUser>? CandidateUsers { get; set; }
}
