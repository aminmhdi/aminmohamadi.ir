using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyWeb.ServiceLayer.Contracts;
using MyWeb.ViewModel.Post;

namespace MyWeb.Controllers
{
  public class RecentController : Controller
  {
    #region Fields

    private readonly IPostService _postService;

    #endregion

    #region Constructor

    public RecentController(IPostService postService)
    {
      _postService = postService;
    }

    #endregion
    [OutputCache(Duration = 86400, VaryByParam = "*")]
    public ActionResult Index()
    {
      return View(_postService.GetForMainPageListForRelated(new PostSearchRequest()));
    }
  }
}