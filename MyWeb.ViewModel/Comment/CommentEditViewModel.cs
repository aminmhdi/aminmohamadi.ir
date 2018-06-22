using System;
using System.ComponentModel.DataAnnotations;

namespace MyWeb.ViewModel.Comment
{
  public class CommentEditViewModel
  {
    public CommentEditViewModel()
    {
      ModifiedOn = DateTime.Now;
    }

    public long Id { get; set; }

    [Display(Name = "متن")]
    public string Body { get; set; }

    [Display(Name = "آی پی")]
    public string Ip { get; set; }

    [Display(Name = "فعال")]
    public bool IsActive { get; set; }

    [Display(Name = "تاریخ ویرایش")]
    public DateTime? ModifiedOn { get; set; }

    [Display(Name = "ویرایش کننده")]
    public virtual long? ModifierId { get; set; }

    public virtual DomainClasses.Entities.Post Post { get; set; }

    public virtual DomainClasses.Entities.User Creator { get; set; }
  }
}
