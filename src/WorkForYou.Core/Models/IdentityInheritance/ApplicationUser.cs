using Microsoft.AspNetCore.Identity;

namespace WorkForYou.Core.Models.IdentityInheritance;

public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; } = string.Empty;
    public string? SkypeLink { get; set; } = string.Empty;
    public string? GithubLink { get; set; } = string.Empty;
    public string? LinkedInLink { get; set; } = string.Empty;
    public string? TelegramLink { get; set; } = string.Empty;
    public string? ImagePath { get; set; } = string.Empty;
    
    public CandidateUser? CandidateUser { get; set; }
    public EmployerUser? EmployerUser { get; set; }
    public IEnumerable<RespondedList>? RespondedList { get; set; }
    public IEnumerable<ChatMessage>? ChatMessages { get; set; }
}
