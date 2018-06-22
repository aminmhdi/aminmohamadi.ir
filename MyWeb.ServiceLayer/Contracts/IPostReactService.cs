using System.Collections.Generic;
using System.Threading.Tasks;
using MyWeb.ViewModel.PostReact;

namespace MyWeb.ServiceLayer.Contracts
{
  public interface IPostsReactService
  {
    Task Create(PostReactInsertViewModel viewModel);
    Task<ICollection<PostReactDetailViewModel>> GetPostReacts(long? id);
    Task<bool> IsInDb(PostReactInsertViewModel viewModel);
    Task ClearCache();
  }
}
