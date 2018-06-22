using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MyWeb.ViewModel.Category
{
    public class CategoryEditViewModel
    {
        public CategoryEditViewModel()
        {
            ModifiedOn = DateTime.Now;
        }

        public long Id { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "*")]
        public string Title { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "تاریخ ویرایش")]
        public DateTime? ModifiedOn { get; set; }

        [Display(Name = "ویرایش کننده")]
        public virtual long? ModifierId { get; set; }

        [Display(Name = "فعال")]
        public bool IsActive { get; set; }
    }
}
