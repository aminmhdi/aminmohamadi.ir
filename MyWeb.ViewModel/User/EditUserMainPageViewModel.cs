using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MyWeb.ViewModel.User
{
  public class EditUserMainPageViewModel
  {
    public long Id { get; set; }

    [Required(ErrorMessage = "لطفا نام نمایشی خود را وارد کنید")]
    [DisplayName("نام و نام خانوادگی")]
    [StringLength(256, ErrorMessage = "نام نمایشی نباید کمتر از 5 حرف و بیتشر از 256 حرف باشد", MinimumLength = 5)]
    //[Remote("IsNameForShowExist", "Account", "", ErrorMessage = "این نام نمایشی قبلا در سیستم ثبت شده است", HttpMethod = "POST")]
    [RegularExpression(@"^[\u0600-\u06FF,\u0590-\u05FF,0-9\s]*$", ErrorMessage = "لطفا فقط ازاعداد و حروف  فارسی استفاده کنید")]
    public string NameForShow { get; set; }

    [Required(ErrorMessage = "لطفا ایمیل خود را وارد کنید")]
    [EmailAddress(ErrorMessage = "ایمیل را به شکل صحیح وارد کنید")]
    [DisplayName("ایمیل")]
    [StringLength(256, ErrorMessage = "حداکثر طول ایمیل 256 حرف است")]
    //[Remote("IsEmailExist", "User", "Management", ErrorMessage = "این ایمیل قبلا در سیستم ثبت شده است", HttpMethod = "POST", AdditionalFields = "Id")]
    public string Email { get; set; }

    [DisplayName("تاریخ تولد")]
    public DateTime? BirthDay { get; set; }

    [DisplayName("تصویر پروفایل")]
    public string AvatarFileName { get; set; }

    public HttpPostedFileBase AvatarImage { get; set; }

    [DisplayName(" آی دی گوگل پلاس")]
    [MaxLength(20, ErrorMessage = "تعداد کاراکتر های آی دی گوگل بیشتر از بیست است")]
    //[Remote("GooglePlusIdExist", "User", "Management", ErrorMessage = "این آدرس قبلا در سیستم ثبت شده است", HttpMethod = "POST", AdditionalFields = "Id")]
    public string GooglePlusId { get; set; }

    [DisplayName("آی دی فیسبوک")]
    [MaxLength(20, ErrorMessage = "تعداد کاراکتر های آی دی فیسبوک بیشتر از بیست است")]
    //[Remote("FaceBookIdExist", "User", "Management", ErrorMessage = "این آدرس قبلا در سیستم ثبت شده است", HttpMethod = "POST", AdditionalFields = "Id")]
    public string FaceBookId { get; set; }

    [DisplayName("درباره من")]
    [MaxLength(256, ErrorMessage = "تعداد کاراکتر های درباره من بیشتر از 256 است")]
    public string Bio { get; set; }

    [DisplayName("موبایل")]
    [MaxLength(20, ErrorMessage = "تعداد کاراکتر های موبایل بیشتر از بیست است")]
    public string PhoneNumber { get; set; }
  }
}
