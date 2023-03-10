namespace WorkForYou.Core.DTOModels.UserDTOs;

public class RefreshGeneralUserDto
{
    public string UserName { get; set; } = string.Empty;
    public string UserRole { get; set; } = string.Empty;
    public string? FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; } = string.Empty;
    public string? SkypeLink { get; set; } = string.Empty;
    public string? GithubLink { get; set; } = string.Empty;
    public string? LinkedInLink { get; set; } = string.Empty;
    public string? TelegramLink { get; set; } = string.Empty;
}
