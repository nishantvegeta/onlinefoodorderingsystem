using System;
using onlinefood.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Builder.Extensions;

namespace onlinefood.Services;

public class EmailService : IEmailService
{

    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        try
        {
            var smtpServer = _config["EmailSettings:Server"];
            var portString = _config["EmailSettings:Port"];

            if (!int.TryParse(portString, out var smtpPort))
            {
                throw new Exception("SMTP port is not configured correctly.");
            }
            var senderName = _config["EmailSettings:SenderName"];
            var senderEmail = _config["EmailSettings:SenderEmail"];
            var username = _config["EmailSettings:Username"];
            var password = _config["EmailSettings:Password"];

            if (string.IsNullOrEmpty(smtpServer) || string.IsNullOrEmpty(senderEmail) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new Exception("Email configuration is missing");
            }

            using (var mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress(senderEmail, senderName);
                mailMessage.To.Add(new MailAddress(email));
                mailMessage.Subject = subject;
                mailMessage.Body = message;
                mailMessage.IsBodyHtml = true;

                using (var smtpClient = new SmtpClient(smtpServer, smtpPort))
                {
                    smtpClient.Credentials = new NetworkCredential(username, password);
                    smtpClient.EnableSsl = true;

                    try
                    {
                        await smtpClient.SendMailAsync(mailMessage);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error sending email: " + ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error in EmailService: " + ex.Message);
        }
    }
}