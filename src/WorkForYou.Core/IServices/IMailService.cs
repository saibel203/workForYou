﻿namespace WorkForYou.Core.IServices;

public interface IMailService
{
    Task SendEmailAsync(string toEmail, string subject, string content, string? link, string? linkText);
    Task SendToAdminEmailAsync(string fromEmail, string subject, string content);
}