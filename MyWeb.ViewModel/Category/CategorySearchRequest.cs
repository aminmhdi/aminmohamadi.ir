using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MyWeb.ViewModel.Common;

namespace MyWeb.ViewModel.Category
{
    public class CategorySearchRequest : BaseSearchRequest
    {
        public CategorySearchRequest()
        {
            CurrentSort = SortByMode.Id;
            SortDirection = SortDirectionMode.Desc;
        }

        [DisplayName("عنوان")]
        public string Title { get; set; }

        [DisplayName("توضیحات")]
        public string Description { get; set; }

        [DisplayName("وضعیت")]
        public ActiveStatus IsActive { get; set; }
    }

    public enum ActiveStatus
    {
        [Display(Name = "فعال")]
        Enable = 1,
        [Display(Name = "غیرفعال")]
        Disable = 0
    }
}
