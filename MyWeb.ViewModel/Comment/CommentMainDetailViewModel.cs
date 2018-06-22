using System;
using System.ComponentModel.DataAnnotations;

namespace MyWeb.ViewModel.Comment
{
    public class CommentMainDetailViewModel
    {
        public long Id { get; set; }

        [Display(Name = "متن")]
        public string Body { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreatedOn { get; set; }

        [Display(Name = "فعال")]
        public bool IsActive { get; set; }

        public virtual long CreatorId { get; set; }

        [Display(Name = "ایجاد کننده")]
        public virtual string CreatorName { get; set; }

        public virtual long PostId { get; set; }

        [Display(Name = "مطلب")]
        public virtual string PostTitle { get; set; }
    }
}
