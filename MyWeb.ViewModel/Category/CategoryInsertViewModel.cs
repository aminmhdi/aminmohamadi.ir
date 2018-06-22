using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MyWeb.ViewModel.Category
{
    public class CategoryInsertViewModel
    {
        public CategoryInsertViewModel()
        {
          IsActive = true;
        }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "*")]
        public string Title { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "ایجاد کننده")]
        [Required(ErrorMessage = "*")]
        public virtual long CreatorId { get; set; }

        [Display(Name = "وضعیت")]
        public bool IsActive { get; set; }
    }
}