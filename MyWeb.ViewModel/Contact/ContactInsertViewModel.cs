using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MyWeb.ViewModel.Contact
{
  public class ContactInsertViewModel
  {

    [Display(Name = "نام")]
    [Required(ErrorMessage = "*")]
    public string Name { get; set; }

    [Display(Name = "عنوان")]
    [Required(ErrorMessage = "*")]
    public string Title { get; set; }

    [Display(Name = "پیام")]
    [Required(ErrorMessage = "*")]
    public string Message { get; set; }
  }
}