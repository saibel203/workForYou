using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using WorkForYou.Core.IOptions;
using WorkForYou.Core.IServices;

namespace WorkForYou.Services;

public class MailService : IMailService
{
    private readonly SendGridOptions _sendGridOptions;
    private readonly ILogger<MailService> _logger;

    public MailService(IOptions<SendGridOptions> sendGridOptions, ILogger<MailService> logger)
    {
        _sendGridOptions = sendGridOptions.Value;
        _logger = logger;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string content, string? link, string? linkText)
    {
        try
        {
            var filePath = Path.Combine(Environment.CurrentDirectory + _sendGridOptions.EmailTemplatePath);
            StringBuilder htmlText = new();

            var htmlFile = await File.ReadAllTextAsync(filePath);
            htmlText.Append(htmlFile);

            htmlText.Replace("#ReceiverEmail#", toEmail);
            htmlText.Replace("#TopicLatter#", subject);
            htmlText.Replace("#ReceiverEmail2#", toEmail);
            htmlText.Replace("#LetterMessage#", content);

            if (link is not null && linkText is not null)
            {
                htmlText.Replace("#Link#", link);
                htmlText.Replace("#LinkText#", linkText);
            }

            var apiKey = _sendGridOptions.ApiKey;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(_sendGridOptions.AdminEmail, _sendGridOptions.AdminUsername);
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject,
                htmlText.ToString(), htmlText.ToString());
            await client.SendEmailAsync(msg);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error send mail");
            throw;
        }
    }

    public async Task SendToAdminEmailAsync(string subject, string content)
     {
         try
         {
             var apiKey = _sendGridOptions.ApiKey;
             var client = new SendGridClient(apiKey);
             var from = new EmailAddress(_sendGridOptions.AdminEmail, _sendGridOptions.AdminUsername);
             var to = new EmailAddress(_sendGridOptions.AdminEmail);
             var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
             await client.SendEmailAsync(msg);
         }
         catch (Exception e)
         {
             _logger.LogError(e, "Error send mail");
             throw;
         }
     }
}