using PulseStore.BLL.Models.Email;

namespace PulseStore.BLL.ExternalServices.EmailSender;

public interface IEmailSenderService
{
    Task SendEmailAsync(EmailMessage message);
}