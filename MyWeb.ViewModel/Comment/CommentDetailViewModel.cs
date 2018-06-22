using System;
using System.ComponentModel.DataAnnotations;

namespace MyWeb.ViewModel.Comment
{
  public class CommentDetailViewModel
  {
    public long Id { get; set; }

    [Display(Name = "متن")]
    public string Body { get; set; }

    [Display(Name = "آی پی")]
    public string Ip { get; set; }

    [Display(Name = "فعال")]
    public bool IsActive { get; set; }

    [Display(Name = "تاریخ ایجاد")]
    public DateTime CreatedOn { get; set; }

    [Display(Name = "تاریخ ویرایش")]
    public DateTime? ModifiedOn { get; set; }

    public virtual long CreatorId { get; set; }

    [Display(Name = "ایجاد کننده")]
    public virtual DomainClasses.Entities.User Creator { get; set; }

    public virtual long? ModifierId { get; set; }

    [Display(Name = "ویرایش کننده")]
    public virtual DomainClasses.Entities.User Modifier { get; set; }

    public virtual long PostId { get; set; }

    [Display(Name = "مطلب")]
    public virtual DomainClasses.Entities.Post Post { get; set; }
  }
}
