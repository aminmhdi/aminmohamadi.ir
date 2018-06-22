using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyWeb.ViewModel.Comment;

namespace MyWeb.ViewModel.Post
{
    public class PostMainDetailViewModel
    {
        public long Id { get; set; }

        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Display(Name = "خلاصه")]
        public string Summary { get; set; }

        [Display(Name = "فعال")]
        public bool IsActive { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreatedOn { get; set; }

        public virtual long CreatorId { get; set; }

        [Display(Name = "ایجاد کننده")]
        public string CreatorName { get; set; }

        public long CategoryId { get; set; }

        [Display(Name = "موضوع")]
        public string CategoryTitle { get; set; }

        [Display(Name = "کلمات کلیدی")]
        public string Keywords { get; set; }

        public int CommentsCount { get; set; }
    }
}
