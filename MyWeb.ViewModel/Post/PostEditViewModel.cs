using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyWeb.ViewModel.Post
{
  public class PostEditViewModel
  {
    public PostEditViewModel()
    {
      ModifiedOn = DateTime.Now;
    }

    public long Id { get; set; }

    [Display(Name = "عنوان")]
    [Required(ErrorMessage = "*")]
    public string Title { get; set; }

    [Display(Name = "خلاصه")]
    [AllowHtml]
    [Required(ErrorMessage = "*")]
    public string Summary { get; set; }

    [Display(Name = "متن اصلی")]
    [AllowHtml]
    [Required(ErrorMessage = "*")]
    public string Body { get; set; }

    [Display(Name = "کلمات کلیدی")]
    public string Keyword { get; set; }

    [Display(Name = "فعال")]
    public bool IsActive { get; set; }

    [Display(Name = "تاریخ ایجاد")]
    [Required(ErrorMessage = "*")]
    [UIHint("MDPersianDateTime")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd hh:mm:ss}")]
    public DateTime CreatedOn { get; set; }

    [Display(Name = "تاریخ ویرایش")]
    public DateTime? ModifiedOn { get; set; }

    [Display(Name = "ویرایش کننده")]
    public virtual long? ModifierId { get; set; }

    [Display(Name = "موضوع")]
    public virtual long CategoryId { get; set; }
    public IEnumerable<SelectListItem> Categories { get; set; }
  }
}
