using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using CaptchaMvc.Attributes;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MyWeb.ServiceLayer.Contracts;
using MyWeb.Utility;
using MyWeb.DomainClasses.Entities;
using MyWeb.ViewModel.Account;
using MyWeb.ViewModel.User;

namespace MyWeb.Controllers
{
  public class AccountController : Controller
  {
    #region Fields

    private readonly IAuthenticationManager _authenticationManager;
    private readonly IApplicationSignInManager _signInManager;
    private readonly IApplicationUserManager _userManager;
    private readonly IApplicationRoleManager _roleManager;
    private readonly IEmailService _emailService;

    #endregion

    #region Constructor

    public AccountController(IApplicationUserManager userManager,
        IApplicationSignInManager signInManager,
        IAuthenticationManager authenticationManager,
        IApplicationRoleManager roleManager,
        IEmailService emailService
       )
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _authenticationManager = authenticationManager;
      _roleManager = roleManager;
      _emailService = emailService;
    }

    #endregion

    #region Validation
    [ChildActionOnly]
    protected ActionResult RedirectToLocal(string returnUrl)
    {
      return Redirect(!string.IsNullOrEmpty(returnUrl) ? returnUrl : "/");

      //return Redirect(Url.IsLocalUrl(returnUrl) ? returnUrl : "/");
    }
    #endregion

    #region ConfirmEmail
    [AllowAnonymous]
    public virtual async Task<ActionResult> ConfirmEmail(int? userId, string code)
    {
      //if(enable confirm email feature then show confirm page)
      //return view("info")
      if (userId == null || code == null)
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

      var result = await _userManager.ConfirmEmailAsync(userId.Value, code);
      if (result.Succeeded)
        return View();

      return RedirectToAction("ReceiveActivatorEmail", "Account");
    }
    #endregion

    #region ExternalLogin
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public virtual ActionResult ExternalLogin(string provider, string returnUrl)
    {
      return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
    }


    [AllowAnonymous]
    public virtual async Task<ActionResult> ExternalLoginCallback(string returnUrl)
    {
      var loginInfo = await _authenticationManager.GetExternalLoginInfoAsync();
      if (loginInfo == null)
      {
        return RedirectToAction("Login");
      }

      // Sign in the user with this external login provider if the user already has a login
      var result = await _signInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
      switch (result)
      {
        case SignInStatus.Success:
          return RedirectToLocal(returnUrl);
        case SignInStatus.LockedOut:
          return View("Lockout");
        case SignInStatus.RequiresVerification:
          return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
        default:
          // If the user does not have an account, then prompt the user to create an account
          ViewBag.ReturnUrl = returnUrl;
          ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
          return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
      }
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public virtual async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
    {
      if (User.Identity.IsAuthenticated)
      {
        return RedirectToAction("Index", "Manage");
      }

      if (ModelState.IsValid)
      {
        // Get the information about the user from the external login provider
        var info = await _authenticationManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
          return View("ExternalLoginFailure");
        }
        var user = new User { UserName = model.Email, Email = model.Email };
        var result = await _userManager.CreateAsync(user);
        if (result.Succeeded)
        {
          result = await _userManager.AddLoginAsync(user.Id, info.Login);
          if (result.Succeeded)
          {
            await _signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            return RedirectToLocal(returnUrl);
          }
        }
        // this.AddErrors(result);
      }

      ViewBag.ReturnUrl = returnUrl;
      return View(model);
    }


    [AllowAnonymous]
    public virtual ActionResult ExternalLoginFailure()
    {
      return View();
    }
    #endregion

    #region ForgetPassword
    [AllowAnonymous]
    public virtual ActionResult ForgotPassword()
    {
      //if(enable forget feature then show forget page)
      //return view("info")
      return View();
    }


    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public virtual async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
      if (!ModelState.IsValid) return View(model);
      var user = await _userManager.FindByEmailAsync(model.Email);
      if (user == null /*|| !(await _userManager.IsEmailConfirmedAsync(user.Id))*/)
      {
        // Don't reveal that the user does not exist or is not confirmed
        return View("ResetPasswordConfirmation");
      }

      var code = await _userManager.GeneratePasswordResetTokenAsync(user.Id);
      if (Request.Url == null) return View("ForgotPasswordConfirmation");
      var callbackUrl = Url.Action("ResetPassword", "Account",
          new { userId = user.Id, code }, protocol: Request.Url.Scheme);

      // TODO
      // Send mail for reset password
      await _emailService.SendEmailAsync(new IdentityMessage
      {
        Body =
        "<table style=\"background-color: #f6f6f6; width: 100%; font-family: calibri; font-size: 16px; direction: rtl; line-height: 18pt; text-align: right;\"> "
          + "<tbody> <tr> <td style=\"display: block !important; max-width: 600px !important; margin: 0 auto !important; clear: both !important;\" width=\"600\"> "
          + "<div style=\"max-width: 600px; margin: 0 auto; display: block; padding: 20px;\"> "
          + "<table style=\"background: #fff; border: 1px solid #e9e9e9; border-radius: 3px;\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\"> "
          + "<tbody> <tr> <td style=\"padding: 20px;\"> <table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\"> "
          + "<tbody> <tr> <td style=\"padding: 0 0 20px; text-align: center\"> <img src=\"http://aminmohamadi.ir/content/images/logo.png\" alt=\"\" /> </td> </tr> <tr> "
          + "<td style=\"padding: 0 0 20px;\"> " + user.NameForShow + " عزیز </td> </tr> <tr> <td style=\"padding: 0 0 20px;\">"
          + " با عرض سلام و احترام <br />"
          + " با کلیک بر روی دکمه زیر میتوانید رمز حساب کاربری خود را تغییر دهید. </td> </tr> <tr> "
          + "<td style=\"padding: 20px; text-align: center\"> <a href=\"" + callbackUrl + "\" style=\"text-decoration: none; color: #FFF; font-size: 14px; background-color:"
          + " #348eda;border: solid #348eda; border-width: 10px 20px; line-height: 2; font-weight: bold; text-align: center; cursor: pointer; display: inline-block; border-radius: 5px; text-transform: capitalize;\">"
          + "تغییر رمز</a> </td> </tr> </tbody> </table> </td> </tr> </tbody> </table> </div> </td> <td></td> </tr> </tbody></table>",
        Destination = user.Email,
        Subject = "بازیابی کلمه عبور اکانت شما - AminMohamadi.ir"
      });

      ViewBag.Message =
        "ایمیل بازیابی کلمه عبور به ایمیل شما ارسال شد، کافیست روی لینک در ایمیل کلیک کنید تا به صفحه تغییر پسورد بروید.";

      return View("ForgotPasswordConfirmation");
    }

    [AllowAnonymous]
    public virtual ActionResult ForgotPasswordConfirmation()
    {
      return View();
    }

    #endregion

    #region Login,LogOff
    [AllowAnonymous]
    //[OutputCache(Duration = 86400, VaryByParam = "none")]
    public virtual ActionResult Login(string returnUrl)
    {
      //if(enable login feature then show login page)
      //return view("info")
      ViewBag.ReturnUrl = returnUrl;
      return View();
    }

    [HttpPost]
    [AllowAnonymous]
    //[ValidateAntiForgeryToken]
    [CaptchaVerify("تصویر امنیتی را درست وارد کنید")]
    public virtual async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
    {
      if (_userManager.CheckIsUserBannedOrDelete(model.UserName))
      {
        this.AddErrors("UserName", "حساب کاربری شما مسدود شده است");
        return View(model);
      }
      //if (!_userManager.IsEmailConfirmedByUserNameAsync(model.UserName))
      //{
      //  ViewBag.Message = "برای ورود به سایت لازم است حساب خود را فعال کنید";

      //  return RedirectToAction("ReceiveActivatorEmail", "Account");
      //}

      if (!ModelState.IsValid)
      {
        return View(model);
      }

      var result = await _signInManager.PasswordSignInAsync
          (model.UserName.ToLower(), model.Password, model.RememberMe, shouldLockout: true);

      switch (result)
      {
        case SignInStatus.Success:
          var user = await _userManager.FindByNameAsync(model.UserName.ToLower());
          user.LastLoginDate = DateTime.Now;
          user.LastIp = Network.GetClientIp();
          await _userManager.UpdateAsync(user);
          return RedirectToLocal(returnUrl);
        case SignInStatus.LockedOut:
          ViewBag.ErrorMessage = string.Format("دقیقه دوباره امتحان کنید {0} حساب شما قفل شد ! لطفا بعد از ",
                 _userManager.DefaultAccountLockoutTimeSpan);
          return View(model);
        case SignInStatus.Failure:
          ViewBag.ErrorMessage = @"نام کاربری یا رمز عبور اشتباه است.";
          return View(model);
        default:
          ViewBag.ErrorMessage = "در این لحظه امکان ورود به  سابت وجود ندارد . مراتب را با مسئولان سایت در میان بگذارید";
          return View(model);
      }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Mvc5Authorize]
    public virtual ActionResult LogOff()
    {
      _authenticationManager.SignOut
          (
              DefaultAuthenticationTypes.ExternalCookie,
              DefaultAuthenticationTypes.ApplicationCookie
          );

      return Redirect("/");
    }

    #endregion

    #region Login,Partial
    [AllowAnonymous]
    //[OutputCache(Duration = 86400, VaryByParam = "none")]
    public virtual ActionResult LoginPartial(string returnUrl)
    {
      //if(enable login feature then show login page)
      //return view("info")
      ViewBag.ReturnUrl = returnUrl;
      return View();
    }

    [HttpPost]
    [AllowAnonymous]
    //[ValidateAntiForgeryToken]
    [CaptchaVerify("تصویر امنیتی را درست وارد کنید")]
    public virtual async Task<ActionResult> LoginPartial(LoginViewModel model, string returnUrl)
    {
      if (_userManager.CheckIsUserBannedOrDelete(model.UserName))
      {
        this.AddErrors("UserName", "حساب کاربری شما مسدود شده است");
        return View(model);
      }
      //if (!_userManager.IsEmailConfirmedByUserNameAsync(model.UserName))
      //{
      //  ViewBag.Message = "برای ورود به سایت لازم است حساب خود را فعال کنید";

      //  return RedirectToAction("ReceiveActivatorEmail", "Account");
      //}

      if (!ModelState.IsValid)
      {
        return View(model);
      }

      var result = await _signInManager.PasswordSignInAsync
          (model.UserName.ToLower(), model.Password, model.RememberMe, shouldLockout: true);

      switch (result)
      {
        case SignInStatus.Success:
          var user = await _userManager.FindByNameAsync(model.UserName.ToLower());
          user.LastLoginDate = DateTime.Now;
          user.LastIp = Network.GetClientIp();
          await _userManager.UpdateAsync(user);
          return Json(new { result = "OK", returnurl = returnUrl }, JsonRequestBehavior.AllowGet);
        //return RedirectToLocal(returnUrl);
        case SignInStatus.LockedOut:
          ViewBag.ErrorMessage = string.Format("دقیقه دوباره امتحان کنید {0} حساب شما قفل شد ! لطفا بعد از ",
                 _userManager.DefaultAccountLockoutTimeSpan);
          return View(model);
        case SignInStatus.Failure:
          ViewBag.ErrorMessage = @"نام کاربری یا رمز عبور اشتباه است.";
          return View(model);
        default:
          ViewBag.ErrorMessage = "در این لحظه امکان ورود به  سابت وجود ندارد . مراتب را با مسئولان سایت در میان بگذارید";
          return View(model);
      }
    }

    #endregion

    #region Register
    [AllowAnonymous]
    public virtual ActionResult Register()
    {
      // if("register is enable")
      // return RedirectToAction("info)
      return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    [CaptchaVerify("تصویر امنیتی را درست وارد کنید")]
    public virtual async Task<ActionResult> Register(RegisterViewModel model)
    {
      #region Validation
      var allRoles = await _roleManager.GetAllRolesAsync();
      var isDefaultForRegister = allRoles.FirstOrDefault(q => q.IsDefaultForRegister);

      if (_userManager.CheckEmailExist(model.Email, null))
        this.AddErrors("Email", "این ایمیل قبلا در سیستم ثبت شده است");

      if (_userManager.CheckUserNameExist(model.UserName, null))
        this.AddErrors("UserName", "این نام کاربری قبلا در سیستم ثبت شده است");

      if (_userManager.CheckNameForShowExist(model.NameForShow, null))
        this.AddErrors("NameForShow", "این نام نمایشی قبلا در سیستم ثبت شده است");

      if (!model.Password.IsSafePasword())
        this.AddErrors("Password", "این کلمه عبور به راحتی قابل تشخیص است");

      if (!model.Password.IsSafePasword())
        this.AddErrors("Password", "این کلمه عبور به راحتی قابل تشخیص است");

      if (isDefaultForRegister == null)
        this.AddErrors("Role", "نقشی برای کاربران ثبت نام شده تعیین نشده است");


      if (!ModelState.IsValid)
      {
        return View(model);
      }

      #endregion

      model.RegistrationIp = Network.GetClientIp();
      model.RegisterDate = DateTime.Now;
      var userId = await _userManager.CreateAsync(model);
      await _userManager.AddToRolesAsync(userId, isDefaultForRegister.Name);

      //TODO
      // Send Confirmation Email
      await _emailService.SendEmailAsync(new IdentityMessage
      {
        Body =
          "<table style=\"background-color: #f6f6f6; width: 100%; font-family: calibri; font-size: 16px; direction: rtl; line-height: 18pt; text-align: right;\"> " +
          "<tbody> <tr> <td style=\"display: block !important; max-width: 600px !important; margin: 0 auto !important; clear: both !important;\" width=\"600\"> " +
          "<div style=\"max-width: 600px; margin: 0 auto; display: block; padding: 20px;\"> " +
          "<table style=\"background: #fff; border: 1px solid #e9e9e9; border-radius: 3px;\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\"> " +
          "<tbody> <tr> <td style=\"padding: 20px;\"> <table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\"> " +
          "<tbody> <tr> <td style=\"padding: 0 0 20px; text-align: center\"> <img src=\"http://aminmohamadi.ir/content/images/logo.png\" alt=\"\" /> </td> </tr> <tr> " +
          "<td style=\"padding: 0 0 20px;\"> " + model.NameForShow +
          " عزیز </td> </tr> <tr> <td style=\"padding: 0 0 20px;\">" +
          " با عرض سلام و احترام <br />" +
          " از این پس میتوانید با استفاده از نام کاربری و رمز عبور زیر جهت ورود به حساب کاربری خود وارد شوید </td> </tr> <tr> " +
          "<td style=\"padding: 0 0 20px;\"> نام کاربری: " + model.UserName + " <br /> کلمه عبور: " + model.Password +
          " </td> </tr> <tr> " +
          "<td style=\"padding: 0 0 20px;\"> برای تغییر کلمه عبور باید کافیست به قسمت حساب کاربری سایت مراجعه کنید. </td> </tr> <tr> " +
          "<td style=\"padding: 20px; text-align: center\"> <a href=\"http://aminmohamadi.ir\" style=\"text-decoration: none; color: #FFF; font-size: 14px; background-color:" +
          " #348eda;border: solid #348eda; border-width: 10px 20px; line-height: 2; font-weight: bold; text-align: center; cursor: pointer; display: inline-block; border-radius: 5px; text-transform: capitalize;\">" +
          "صفحه اصلی سایت</a> </td> </tr> </tbody> </table> </td> </tr> </tbody> </table> </div> </td> <td></td> </tr> </tbody></table>",

        Destination = model.Email,
        Subject = "ایجاد حساب کاربری جدید - AminMohamadi.ir"
      });



      ViewBag.Message = "حساب کاربری شما با موفقیت ایجاد شد. ایمیلی حاوی نام کاربری و رمز عبور شما برای شما فرستاده شد. " +
                    "برای مشاهده اطلاعات کاربری به ایمیل خود مراجعه کنید.";

      return View("RegisterSuccessfully");
    }
    #endregion

    #region ResetPassword

    [HttpGet]
    [AllowAnonymous]
    public virtual ActionResult ResetPassword(string code)
    {
      //if(enable resetpass feature then show resetpass page)
      //return view("info")
      if (code == null) return HttpNotFound();

      return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    [CaptchaVerify("تصویر امنیتی را درست وارد کنید")]
    public virtual async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
    {
      if (!model.Password.IsSafePasword())
        this.AddErrors("Password", "این کلمه عبور به راحتی قابل تشخیص است");
      if (!ModelState.IsValid)
      {
        return View(model);
      }
      var user = await _userManager.FindByEmailAsync(model.Email.ToLower());
      if (user == null)
      {
        // Don't reveal that the user does not exist
        return Redirect("/");
      }
      var result = await _userManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
      if (result.Succeeded)
      {
        await _signInManager.SignInAsync(user, false, false);

        await _emailService.SendEmailAsync(new IdentityMessage
        {
          Body =
            "<table style=\"background-color: #f6f6f6; width: 100%; font-family: calibri; font-size: 16px; direction: rtl; line-height: 18pt; text-align: right;\">" +
            "<tbody> <tr> <td style=\"display: block !important; max-width: 600px !important; margin: 0 auto !important; clear: both !important;\" width=\"600\"> " +
            "<div style=\"max-width: 600px; margin: 0 auto; display: block; padding: 20px;\"> " +
            "<table style=\"background: #fff; border: 1px solid #e9e9e9; border-radius: 3px;\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\"> " +
            "<tbody> <tr> <td style=\"padding: 20px;\"> <table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\"> " +
            "<tbody> <tr> <td style=\"padding: 0 0 20px; text-align: center\"> <img src=\"http://aminmohamadi.ir/content/images/logo.png\" alt=\"\" /> </td> </tr> <tr> " +
            "<td style=\"padding: 0 0 20px;\"> " + user.NameForShow +
            " عزیز </td> </tr> <tr> <td style=\"padding: 0 0 20px;\">" +
            " با عرض سلام و احترام <br />" +
            " از این پس میتوانید با استفاده از نام کاربری و رمز عبور زیر جهت ورود به حساب کاربری خود وارد شوید </td> </tr> <tr> " +
            "<td style=\"padding: 0 0 20px;\"> نام کاربری: " + user.UserName + " <br /> کلمه عبور: " + model.Password +
            " </td> </tr> <tr> " +
            "<td style=\"padding: 0 0 20px;\"> برای تغییر کلمه عبور باید کافیست به قسمت حساب کاربری سایت مراجعه کنید. </td> </tr> <tr> " +
            "<td style=\"padding: 20px; text-align: center\"> <a href=\"http://aminmohamadi.ir\" style=\"text-decoration: none; color: #FFF; font-size: 14px; background-color:" +
            " #348eda;border: solid #348eda; border-width: 10px 20px; line-height: 2; font-weight: bold; text-align: center; cursor: pointer; display: inline-block; border-radius: 5px; text-transform: capitalize;\">" +
            "صفحه اصلی سایت</a> </td> </tr> </tbody> </table> </td> </tr> </tbody> </table> </div> </td> <td></td> </tr> </tbody></table>",

          Destination = model.Email,
          Subject = "کلمه عبور جدید - AminMohamadi.ir"
        });

        return View("ResetPasswordConfirmation");
      }
      this.AddErrors(result);

      ViewBag.ErrorMessage = ModelState.GetListOfErrors();
      return View(model);
    }

    [AllowAnonymous]
    public virtual ActionResult ResetPasswordConfirmation()
    {
      return View();
    }
    #endregion

    #region ReceiveActivatorEmail
    [AllowAnonymous]
    public virtual ActionResult ReceiveActivatorEmail()
    {
      //if(enable receiveactivator feature then show receiveactivator page)
      //return view("info")
      return View();
    }

    [HttpPost]
    [AllowAnonymous]
    // [CheckReferrer]
    [ValidateAntiForgeryToken]
    [CaptchaVerify("تصویر امنیتی را درست وارد کنید")]
    public virtual async Task<ActionResult> ReceiveActivatorEmail(ActivationEmailViewModel viewModel)
    {
      if (!_userManager.IsEmailAvailableForConfirm(viewModel.Email))
        this.AddErrors("Email", "ایمیل مورد نظر یافت نشد");
      if (_userManager.CheckIsUserBannedOrDeleteByEmail(viewModel.Email))
        this.AddErrors("Email", "اکانت شما مسدود شده است");
      if (!ModelState.IsValid)
        return View(viewModel);
      var user = await _userManager.FindByEmailAsync(viewModel.Email);

      // TODO
      // Send Confirmation Email
      //_emailService.SendEmail(new IdentityMessage
      //{
      //  Body =
      //      "<table style=\"background-color: #f6f6f6; width: 100%; font-family: calibri; font-size: 13px; direction: rtl\"> " +
      //      "<tbody> <tr> <td style=\"display: block !important; max-width: 600px !important; margin: 0 auto !important; clear: both !important;\" width=\"600\"> " +
      //      "<div style=\"max-width: 600px; margin: 0 auto; display: block; padding: 20px;\"> " +
      //      "<table style=\"background: #fff; border: 1px solid #e9e9e9; border-radius: 3px;\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\"> " +
      //      "<tbody> <tr> <td style=\"padding: 20px;\"> <table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\"> " +
      //      "<tbody> <tr> <td style=\"padding: 0 0 20px; text-align: center\"> <img src=\"http://aminmohamadi.ir/content/images/logo.png\" alt=\"\" /> </td> </tr> <tr> " +
      //      "<td style=\"padding: 0 0 20px;\"> " + user.NameForShow +
      //      " عزیز </td> </tr> <tr> <td style=\"padding: 0 0 20px;\">" +
      //      " با عرض سلام و احترام <br />" +
      //      " از این پس میتوانید با استفاده از نام کاربری و رمز عبور زیر جهت ورود به حساب کاربری خود وارد شوید </td> </tr> <tr> " +
      //      "<td style=\"padding: 0 0 20px;\"> نام کاربری: " + user.UserName + " <br /> کلمه عبور: " + user.Password +
      //      " </td> </tr> <tr> " +
      //      "<td style=\"padding: 0 0 20px;\"> برای تغییر کلمه عبور باید کافیست به قسمت حساب کاربری سایت مراجعه کنید. </td> </tr> <tr> " +
      //      "<td style=\"padding: 20px; text-align: center\"> <a href=\"http://aminmohamadi.ir\" style=\"text-decoration: none; color: #FFF; font-size: 14px; background-color:" +
      //      " #348eda;border: solid #348eda; border-width: 10px 20px; line-height: 2; font-weight: bold; text-align: center; cursor: pointer; display: inline-block; border-radius: 5px; text-transform: capitalize;\">" +
      //      "صفحه اصلی سایت</a> </td> </tr> </tbody> </table> </td> </tr> </tbody> </table> </div> </td> <td></td> </tr> </tbody></table>",

      //  Destination = user.Email,
      //  Subject = "فعال سازی حساب کاربری - AminMohamadi.ir"
      //});

      ViewBag.Message = "ایمیلی تحت عنوان فعال سازی حساب کاربری به آدرس ایمیل شما ارسال گردید";

      return RedirectToAction("ReceiveActivatorEmail", "Account");
    }

    #endregion

    #region Edit

    [HttpGet]
    public async Task<ActionResult> Edit()
    {
      if (!string.IsNullOrEmpty(User.Identity.GetUserId()))
      {
        var user = await _userManager.GetUserForMainById(long.Parse(User.Identity.GetUserId()));
        return View(user);
      }
      return RedirectToLocal("/");
    }

    [HttpPost]
    [CaptchaVerify("تصویر امنیتی را درست وارد کنید")]
    public async Task<ActionResult> Edit(EditUserMainPageViewModel model)
    {
      model.Id = long.Parse(User.Identity.GetUserId());

      if (model.AvatarImage != null && model.AvatarImage.ContentLength > 0)
      {
        var avatarName = model.Id.ToString();
        avatarName = this.UploadAvatarFile(model.AvatarImage, avatarName);
        model.AvatarFileName = avatarName;
      }

      var status = await _userManager.EditUserMainPage(model);

      var user = await _userManager.GetUserForMainById(long.Parse(User.Identity.GetUserId()));

      if (status)
      {
        ViewBag.SuccessMessage = "ویرایش با موفقیت انجام شد.";
        return View(user);
      }

      ViewBag.ErrorMessage = "ویرایش انجام نشد.";
      return View(user);

      //return RedirectToLocal(status ? "/Account/Edit" : "/");
    }
    #endregion

    #region AdminMenu
    [OverrideAuthorization]
    [Authorize]
    public virtual ActionResult AdminMenu()
    {
      //get user info 
      //get adminMneu page content from site setting

      return View();
    }
    #endregion

    #region Validation

    [HttpPost]
    [AllowAnonymous]
    [OutputCache(Location = OutputCacheLocation.None, NoStore = true, Duration = 0, VaryByParam = "*")]
    public virtual JsonResult IsEmailAvailable(string email)
    {
      return _userManager.IsEmailAvailableForConfirm(email) ? Json(true) : Json(false);
    }

    [HttpPost]
    [AllowAnonymous]
    [OutputCache(Location = OutputCacheLocation.None, NoStore = true, Duration = 0, VaryByParam = "*")]
    public virtual JsonResult CheckPassword(string password)
    {
      return password.IsSafePasword() ? Json(true) : Json(false);
    }
    [HttpPost]
    [AllowAnonymous]
    [OutputCache(Location = OutputCacheLocation.None, NoStore = true, Duration = 0, VaryByParam = "*")]
    public virtual JsonResult IsNameForShowExist(string nameForShow, int? id)
    {
      return _userManager.CheckNameForShowExist(nameForShow, id) ? Json(false) : Json(true);
    }
    [HttpPost]
    [AllowAnonymous]
    [OutputCache(Location = OutputCacheLocation.None, NoStore = true, Duration = 0, VaryByParam = "*")]
    public virtual JsonResult IsEmailExist(string email, int? id)
    {
      var check = _userManager.CheckEmailExist(email, id);
      return check ? Json(false) : Json(true);
    }

    [HttpPost]
    [AllowAnonymous]
    [OutputCache(Location = OutputCacheLocation.None, NoStore = true, Duration = 0, VaryByParam = "*")]
    public virtual JsonResult IsUserNameExist(string userName, int? id)
    {
      return _userManager.CheckUserNameExist(userName, id) ? Json(false) : Json(true);
    }
    #endregion

    #region Private

    //public async Task SendConfirmationEmail(string email, long userId)
    //{
    //}
    #endregion
  }
}