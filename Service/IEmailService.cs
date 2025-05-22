using System.Net.Mail;
using System.Net;

namespace BarberShopSystem.Service;

public interface IEmailService
{
    public Task SendEmailAsync(string toEmail, string subject, string body);
}
