using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using MyWeb.ViewModel.Category;
using MyWeb.ViewModel.Comment;
using MyWeb.ViewModel.Dashboard;
using MyWeb.ViewModel.Post;

namespace MyWeb.ServiceLayer.Contracts
{
  public interface IDashboardService
  {
    //PostInsertViewModel GetForCreate();
    //Task Create(PostInsertViewModel model);
    //Task Edit(PostEditViewModel model);
    //Task<PostDetailViewModel> GetDetails(long? id);
    //Task<PostDetailViewModel> GetForMainPageDetails(long? id);
    //Task<PostEditViewModel> GetForEdit(long? id);
    //Task DeleteAsync(long? id);
    //Task<PostListViewModel> GetPagedListAsync(PostSearchRequest request);
    //PostListViewModel GetForMainPageListForRelated(PostSearchRequest request);
    //Task<PostListViewModel> GetForMainPagePagedListForAsync(PostSearchRequest request);
    //IEnumerable<string> GetKeywords();
    Task<string> CountOfWebsiteViewForToDayInHours();

    Task<string> CountOfWebsiteViewIn30DaysAgo();

    Task<string> CountOfWebsiteViewIn12MonthsAgo();

    Task<string> CountOfWebsiteViewIn7YearsAgo();

    Task<CommentMainListViewModel> LastComments();

    Task<List<MostViewedPostDashboardViewModel>> MostViewedPosts();

    Task<string> ViewersDevice();

    Task<WebsiteStatistics> WebsiteStatistics();
    //Task<bool> IsInDb(long id);
    //Task ClearCache();
  }
}
