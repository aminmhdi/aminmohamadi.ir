using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using MyWeb.ViewModel.Category;
using MyWeb.ViewModel.Post;

namespace MyWeb.ServiceLayer.Contracts
{
    public interface IPostService
    {
        PostInsertViewModel GetForCreate();
        Task Create(PostInsertViewModel model);
        Task Edit(PostEditViewModel model);
        Task<PostDetailViewModel> GetDetails(long? id);
        Task<PostDetailViewModel> GetForMainPageDetails(long? id);
        Task<PostEditViewModel> GetForEdit(long? id);
        Task DeleteAsync(long? id);
        Task<PostListViewModel> GetPagedListAsync(PostSearchRequest request);
        PostRelatedOrRecentListViewModel GetForMainPageListForRelated(PostSearchRequest request);
        Task<PostMainListViewModel> GetForMainPagePagedListForAsync(PostSearchRequest request);
        Task<PostMainTitleAndIdViewModel> GetForMainIdAndTitle(long? id);
        IEnumerable<string> GetKeywords();
        Task<int> CountOfAll();
        Task<bool> IsInDb(long id);
        Task ClearCache();
    }
}
