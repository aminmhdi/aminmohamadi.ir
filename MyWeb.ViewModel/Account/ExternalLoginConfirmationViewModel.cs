using System.ComponentModel.DataAnnotations;

namespace MyWeb.ViewModel.Account
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required(ErrorMessage = "وارد کردن ایمیل ضروریست")]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }
    }
}