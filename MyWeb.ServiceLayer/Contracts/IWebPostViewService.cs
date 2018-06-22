using System.Collections.Generic;
using System.Threading.Tasks;
using MyWeb.ViewModel.Dashboard;
using MyWeb.ViewModel.WebPostView;

namespace MyWeb.ServiceLayer.Contracts
{
  public interface IWebPostViewService
  {
    Task Create(WebPostViewInsertViewModel viewModel);
    Task<List<MostViewedPostDashboardViewModel>> GetMostViewPosts();
    Task<bool> IsInDb(WebPostViewInsertViewModel viewModel);
    Task ClearCache();
  }
}
