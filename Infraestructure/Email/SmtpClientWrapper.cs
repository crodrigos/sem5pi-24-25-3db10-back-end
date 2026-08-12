using System.Net;
using System.Net.Mail;

public class SmtpClientWrapper : ISmtpClientWrapper
{
    private readonly SmtpClient _smtpClient;

    public SmtpClientWrapper(string host, int port, string user, string password)
    {
        _smtpClient = new SmtpClient(host, port)
        {
            Credentials = new NetworkCredential(user, password),
            EnableSsl = true
        };
    }

    public async Task SendMailAsync(MailMessage message)
    {
        await _smtpClient.SendMailAsync(message);
    }
}