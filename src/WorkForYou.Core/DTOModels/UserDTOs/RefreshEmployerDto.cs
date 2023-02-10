namespace WorkForYou.Core.DTOModels.UserDTOs;

public class RefreshEmployerDto
{
    public string Username { get; set; } = string.Empty;
    public string? CompanyPosition { get; set; } = string.Empty;
    public string? CompanyName { get; set; } = string.Empty;
    public string? CompanySiteLink { get; set; } = string.Empty;
    public string? DoyCompanyLink { get; set; } = string.Empty;
    public string? AboutCompany { get; set; } = string.Empty;
}
