namespace WorkForYou.Core.IOptions;

public class SendGridOptions
{
    public string AdminEmail { get; set; } = string.Empty;
    public string AdminUsername { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string EmailTemplatePath { get; set; } = string.Empty;
}
