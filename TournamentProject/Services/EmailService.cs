using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using TournamentProject.Services;

public class EmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings?.Value ?? throw new ArgumentNullException(nameof(emailSettings));

        if (string.IsNullOrWhiteSpace(_emailSettings.SmtpServer))
            throw new ArgumentNullException(nameof(_emailSettings.SmtpServer), "SMTP server must be specified in EmailSettings.");
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("Sobhan Khedry", _emailSettings.FromEmail));
        emailMessage.To.Add(new MailboxAddress("Recipient", email));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart("html") { Text = message };

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
    }
}


