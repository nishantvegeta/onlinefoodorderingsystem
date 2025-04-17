using System;

namespace onlinefood.Services.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string email, string subject, string message);
}
