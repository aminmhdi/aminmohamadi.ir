using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using MyWeb.ServiceLayer.Contracts;

namespace MyWeb.ServiceLayer.Common
{
  public class EmailService : IIdentityMessageService, IEmailService
  {
    public Task SendAsync(IdentityMessage message)
    {
      return Task.FromResult(0);
    }

    public async Task<bool> SendEmailAsync(IdentityMessage message)
    {
      try
      {
        var mailMessage = new MailMessage();
        mailMessage.To.Add(new MailAddress(message.Destination.Trim()));
        //mailMessage.CC.Add(new MailAddress("vmp351@yahoo.com"));
        mailMessage.From = new MailAddress("info@aminmohamadi.ir");
        mailMessage.Subject = message.Subject;
        mailMessage.Body = message.Body;
        mailMessage.IsBodyHtml = true;

        using (var smtp = new SmtpClient())
        {
          smtp.Credentials = new NetworkCredential
          {
            UserName = "info@aminmohamadi.ir",  // replace with valid value
            Password = "x2Ur8p42rH"  // replace with valid value
          };
          smtp.Host = "mail.aminmohamadi.ir";
          smtp.Port = 587;
          smtp.EnableSsl = false;
          await smtp.SendMailAsync(mailMessage);
        }
        return true;
      }
      catch (SmtpException)
      {
        return false;
      }
    }
  }
}
