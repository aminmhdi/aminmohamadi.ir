using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MyWeb.ServiceLayer.Contracts;
using MyWeb.ViewModel.Post;

namespace MyWeb.Controllers
{
  public class PageController : Controller
  {
    // GET: Page
    #region Fields

    private readonly IPostService _postService;

    #endregion

    #region Constructor

    public PageController(IPostService postService)
    {
      _postService = postService;
    }

    #endregion
    public async Task<ActionResult> Index(int id)
    {
      if (id > 1)
      {
        return View(await _postService.GetForMainPagePagedListForAsync(new PostSearchRequest
        {
          PageIndex = id
        }));
      }
      if (id == 1)
      {
        return Redirect("/");
      }
      throw new HttpException(404, "Page not Found");
    }
  }
}