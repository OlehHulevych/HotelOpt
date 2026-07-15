using HotelOpt.Application.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace HotelOpt.Infrastructure.Services;

public class EmailService:IEmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }


    public async Task SendAsync(string to, string subject, string body)
    {
        var smtpServer = "smtp.gmail.com";
        var port = 587;
        var fromEmail = _config["Email:SenderEmail"];
        var password = _config["Email:Password"];
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress("HotelOpt", fromEmail));
        email.To.Add(new MailboxAddress("Employee",to));
        email.Subject = subject;
        email.Body = new TextPart("html") {Text = body};

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(smtpServer, port, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(fromEmail, password);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}