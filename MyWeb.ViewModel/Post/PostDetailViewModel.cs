using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MyWeb.DomainClasses.Entities;
using MyWeb.ViewModel.Comment;
using MyWeb.ViewModel.PostReact;

namespace MyWeb.ViewModel.Post
{
  public class PostDetailViewModel
  {
    public long Id { get; set; }

    [Display(Name = "عنوان")]
    public string Title { get; set; }

    [Display(Name = "خلاصه")]
    public string Summary { get; set; }

    [Display(Name = "متن کامل")]
    public string Body { get; set; }

    [Display(Name = "کلمات کلیدی")]
    public string Keyword { get; set; }

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

    public virtual long CategoryId { get; set; }

    [Display(Name = "ایجاد کننده")]
    public virtual DomainClasses.Entities.Category Category { get; set; }

    public virtual ICollection<CommentDetailViewModel> Comments { get; set; }

    public virtual ICollection<PostReactDetailViewModel> PostReacts { get; set; }
  }
}
