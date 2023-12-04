using PulseStore.BLL.Models.Email;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace PulseStore.BLL.ExternalServices.EmailSender;

public class EmailSenderService : IEmailSenderService
{
    private readonly EmailConfiguration _emailConfig;

    public EmailSenderService(IOptions<EmailConfiguration> emailConfigOptions)
    {
        _emailConfig = emailConfigOptions.Value;
    }

    public async Task SendEmailAsync(EmailMessage message)
    {
        var emailMessage = CreateEmailMessage(message);
        await SendAsync(emailMessage);
    }

    private MimeMessage CreateEmailMessage(EmailMessage message)
    {
        return new MimeMessage
        {
            From = { new MailboxAddress(_emailConfig.FromName, _emailConfig.From) },
            To = { message.To },
            Subject = message.Subject,
            Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message.Content
            }
        };
    }

    private async Task SendAsync(MimeMessage emailMessage)
    {
        using (var client = new SmtpClient())
        {
            try
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, _emailConfig.UseSsl);
                await client.AuthenticateAsync(_emailConfig.From, _emailConfig.Password);
                await client.SendAsync(emailMessage);
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
        }
    }
}