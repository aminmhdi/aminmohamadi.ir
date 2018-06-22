using System.ComponentModel.DataAnnotations;

namespace MyWeb.ViewModel.Comment
{
  public class CommentInsertViewModel
  {
    public CommentInsertViewModel()
    {
      IsActive = false;
    }

    [StringLength(1000, ErrorMessage = "حداکثر طول نظر 1000 حرف است")]
    [Required(ErrorMessage = "لطفا نظر خود را وارد کنید")]
    [Display(Name = "نظر شما")]
    public string Body { get; set; }

    [Display(Name = "آی پی")]
    public string Ip { get; set; }

    [Display(Name = "فعال")]
    public bool IsActive { get; set; }

    [Display(Name = "ایجاد کننده")]
    public virtual long CreatorId { get; set; }

    [Display(Name = "نوشته")]
    public virtual long PostId { get; set; }
  }
}