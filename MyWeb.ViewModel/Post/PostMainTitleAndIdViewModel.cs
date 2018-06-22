using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyWeb.ViewModel.Comment;

namespace MyWeb.ViewModel.Post
{
    public class PostMainTitleAndIdViewModel
    {
        public long Id { get; set; }

        [Display(Name = "عنوان")]
        public string Title { get; set; }
    }
}
