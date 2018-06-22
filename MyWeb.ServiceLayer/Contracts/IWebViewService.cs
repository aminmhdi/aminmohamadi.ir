using System.Threading.Tasks;
using MyWeb.ViewModel.WebView;

namespace MyWeb.ServiceLayer.Contracts
{
  public interface IWebViewService
  {
    Task Create(WebViewInsertViewModel viewModel);
   
    Task<bool> IsInDb(WebViewInsertViewModel viewModel);
    Task ClearCache();
  }
}
