using Microsoft.AspNetCore.Http;
using MimeKit;

namespace PulseStore.BLL.Models.Email;

public class EmailMessage
{
    public MailboxAddress To { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }

    public EmailMessage(string name, string address, string subject, string content)
    {
        To = new MailboxAddress(name, address);
        Subject = subject;
        Content = content;
    }
}