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
  public class RelatedController : Controller
  {
    #region Fields

    private readonly IPostService _postService;

    #endregion

    #region Constructor

    public RelatedController(IPostService postService)
    {
      _postService = postService;
    }

    #endregion

    [OutputCache(Duration = 86400, VaryByParam = "*")]
    public ActionResult Index(long? categoryId, long postId)
    {
      return View(_postService.GetForMainPageListForRelated(new PostSearchRequest
      {
        CategoryId = categoryId.Value,
        PostId = postId
      }));
    }

  }
}