using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using MyWeb.DomainClasses.Entities;

namespace MyWeb.ViewModel.Post
{
    public class PostInsertViewModel
    {
        public PostInsertViewModel()
        {
            IsActive = true;
        }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "*")]
        public string Title { get; set; }

        [Display(Name = "خلاصه")]
        [Required(ErrorMessage = "*")]
        [AllowHtml]
        public string Summary { get; set; }

        [Display(Name = "متن اصلی")]
        [Required(ErrorMessage = "*")]
        [AllowHtml]
        public string Body { get; set; }

        [Display(Name = "کلمات کلیدی")]
        public string Keyword { get; set; }

        [Display(Name = "فعال")]
        public bool IsActive { get; set; }

        [Display(Name = "ایجاد کننده")]
        [Required(ErrorMessage = "*")]
        public virtual long CreatorId { get; set; }

        [Display(Name = "موضوع")]
        public virtual long CategoryId { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}