using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MyWeb.ViewModel.Common;

namespace MyWeb.ViewModel.Post
{
  public class PostSearchRequest : BaseSearchRequest
  {
    public PostSearchRequest()
    {
      CurrentSort = SortByMode.Id;
      SortDirection = SortDirectionMode.Desc;
    }

    public long PostId { get; set; }

    [DisplayName("عنوان")]
    public string Title { get; set; }

    [DisplayName("کلمه کلیدی")]
    public string Keyword { get; set; }

    [DisplayName("نویسنده")]
    public long CreatorId { get; set; }
    public IEnumerable<SelectListItem> Creators { get; set; }

    [DisplayName("موضوع")]
    public long CategoryId { get; set; }

    public IEnumerable<SelectListItem> Categories { get; set; }

    [DisplayName("وضعیت")]
    public ActiveStatus IsActive { get; set; }

    public string CategoryTitle { get; set; }
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
