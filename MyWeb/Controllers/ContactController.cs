using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using CaptchaMvc.Attributes;
using Microsoft.AspNet.Identity;
using MyWeb.ServiceLayer.Contracts;
using MyWeb.ViewModel.Contact;
using MyWeb.ViewModel.Post;

namespace MyWeb.Controllers
{
  public class ContactController : Controller
  {
    #region Fields

    private readonly IEmailService _emailService;

    #endregion

    #region Constructor

    public ContactController(IEmailService emailService)
    {
      _emailService = emailService;
    }

    #endregion

    [HttpGet]
    public ActionResult Index()
    {
      return View();
    }

    [HttpPost]
    [CaptchaVerify("تصویر امنیتی را درست وارد کنید")]
    public async Task<ActionResult> Index(ContactInsertViewModel viewModel)
    {
      //var message = new IdentityMessage
      //{
      //  Body = viewModel.Name + @"<br/>" + viewModel.Message,
      //  Destination = "info@aminmohamadi.ir",
      //  Subject = viewModel.Title + " - AminMohamadi.ir"
      //};

      //_emailService.SendEmail(message);

      var body = viewModel.Name + @"<br/>" + viewModel.Message;
      var message = new MailMessage();
      message.To.Add(new MailAddress("info@aminmohamadi.ir"));
      message.CC.Add(new MailAddress("vmp351@yahoo.com"));
      message.From = new MailAddress("info@aminmohamadi.ir");
      message.Subject = viewModel.Title + " - AminMohamadi.ir";
      message.Body = string.Format(body);
      message.IsBodyHtml = true;

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
        await smtp.SendMailAsync(message);
      }

      return View("ContactSentSuccessfully");
    }
  }
}