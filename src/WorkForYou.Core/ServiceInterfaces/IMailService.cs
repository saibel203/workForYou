namespace WorkForYou.Core.ServiceInterfaces;

public interface IMailService
{
    Task SendEmailAsync(string toEmail, string subject, string content, string? link, string? linkText);
    Task SendToAdminEmailAsync(string subject, string content);
}