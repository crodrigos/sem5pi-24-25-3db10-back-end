using System.Net.Mail;

public interface ISmtpClientWrapper
{
    Task SendMailAsync(MailMessage message);
}