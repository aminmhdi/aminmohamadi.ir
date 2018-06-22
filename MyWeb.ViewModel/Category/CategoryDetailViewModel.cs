using System;
using System.ComponentModel.DataAnnotations;

namespace MyWeb.ViewModel.Category
{
  public class CategoryDetailViewModel
  {
    public long Id { get; set; }

    [Display(Name = "عنوان")]
    public string Title { get; set; }

    [Display(Name = "توضیحات")]
    public string Description { get; set; }

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

    public virtual long? CategoryId { get; set; }

    [Display(Name = "ایجاد کننده")]
    public virtual DomainClasses.Entities.Category Creatory { get; set; }
  }
}
