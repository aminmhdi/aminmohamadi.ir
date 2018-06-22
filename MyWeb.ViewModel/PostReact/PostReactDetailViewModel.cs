using System;

namespace MyWeb.ViewModel.PostReact
{
  public class PostReactDetailViewModel
  {
    public PostReactDetailViewModel()
    {
      CreatedOn = DateTime.Now;
    }

    public DateTime CreatedOn { get; set; }
    public virtual long PostId { get; set; }
    public virtual long UserId { get; set; }
    public virtual bool Like { get; set; }
  }
}