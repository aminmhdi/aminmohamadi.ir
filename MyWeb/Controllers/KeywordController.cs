using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyWeb.ServiceLayer.Contracts;

namespace MyWeb.Controllers
{
  public class KeywordController : Controller
  {
    #region Fields

    private readonly IPostService _postService;

    #endregion

    #region Constructor

    public KeywordController(IPostService postService)
    {
      _postService = postService;
    }

    #endregion
    [OutputCache(Duration = 86400, VaryByParam = "*")]
    public ActionResult Index()
    {
      return View(_postService.GetKeywords());
    }
  }
}