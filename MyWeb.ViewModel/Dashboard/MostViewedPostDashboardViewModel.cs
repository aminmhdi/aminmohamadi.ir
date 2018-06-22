using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MyWeb.ViewModel.Comment;
using MyWeb.ViewModel.PostReact;

namespace MyWeb.ViewModel.Dashboard
{
  public class MostViewedPostDashboardViewModel
  {
    public long Id { get; set; }

    [Display(Name = "عنوان")]
    public string Title { get; set; }

    [Display(Name = "فعال")]
    public bool IsActive { get; set; }

    [Display(Name = "تاریخ ایجاد")]
    public DateTime CreatedOn { get; set; }

    [Display(Name = "ایجاد کننده")]
    public virtual string CreatorName { get; set; }

    [Display(Name = "موضوع")]
    public virtual string CategoryName { get; set; }

    public virtual ICollection<DomainClasses.Entities.Comment> Comments { get; set; }

    public virtual ICollection<DomainClasses.Entities.PostReact> PostReacts { get; set; }

    public int ViewCount { get; set; }
  }
}
