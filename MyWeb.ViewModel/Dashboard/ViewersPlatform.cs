using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MyWeb.ViewModel.Comment;
using MyWeb.ViewModel.PostReact;

namespace MyWeb.ViewModel.Dashboard
{
  public class ViewersPlatform
  {
    [Display(Name = "عنوان")]
    public string label { get; set; }

    public int value { get; set; }
  }
}
