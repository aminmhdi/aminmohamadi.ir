using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace MyWeb.ServiceLayer.Contracts
{
  public interface IEmailService
  {
    Task<bool> SendEmailAsync(IdentityMessage message);

  }
}
