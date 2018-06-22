using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CaptchaMvc.Attributes;
using Microsoft.AspNet.Identity;
using MyWeb.ServiceLayer.Contracts;
using MyWeb.Utility;
using MyWeb.ViewModel.Comment;
using MyWeb.ViewModel.Post;

namespace MyWeb.Controllers
{
  public class SearchController : Controller
  {
    // GET: Post

    #region Fields

    private readonly IPostService _postService;

    #endregion

    #region Constructor

    public SearchController(IPostService postService)
    {
      _postService = postService;
    }

    #endregion

    public async Task<ActionResult> Index(string id, int? page = 1)
    {
      var posts = await _postService.GetForMainPagePagedListForAsync(new PostSearchRequest
      {
        Term = id,
        PageIndex = page.Value
      });

      if (!string.IsNullOrEmpty(posts.SearchRequest.Term))
        return View(posts);
      throw new HttpException(404, "Page not Found");

    }
  }
}