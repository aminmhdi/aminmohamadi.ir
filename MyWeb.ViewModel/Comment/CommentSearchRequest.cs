using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MyWeb.ViewModel.Common;

namespace MyWeb.ViewModel.Comment
{
  public class CommentSearchRequest : BaseSearchRequest
  {
    public CommentSearchRequest()
    {
      CurrentSort = SortByMode.Id;
      SortDirection = SortDirectionMode.Desc;
    }

    [DisplayName("متن")]
    public string Body { get; set; }

    [DisplayName("نوشته")]
    public long? Post { get; set; }

    public long? Id { get; set; }

    [DisplayName("وضعیت")]
    public ActiveStatus IsActive { get; set; }
  }

  public enum ActiveStatus
  {
    [Display(Name = "همه")]
    All = 0,
    [Display(Name = "فعال")]
    Enable = 1,
    [Display(Name = "غیرفعال")]
    Disable = 2
  }
}
