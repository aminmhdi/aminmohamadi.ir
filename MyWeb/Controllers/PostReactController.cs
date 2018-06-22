using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using MyWeb.ServiceLayer.Contracts;
using MyWeb.ViewModel.PostReact;

namespace MyWeb.Controllers
{
  public class PostReactController : Controller
  {
    #region Fields

    private readonly IPostsReactService _postReactService;

    #endregion

    #region Constructor

    public PostReactController(IPostsReactService postReactService)
    {
      _postReactService = postReactService;
    }

    #endregion

    [HttpGet]
    public ActionResult Index(ICollection<PostReactDetailViewModel> postReactDetailViewModels, long postId)
    {
      ViewBag.PostId = postId;
      return View(postReactDetailViewModels);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Index(PostReactInsertViewModel viewModel)
    {
      if (viewModel.UserId == 0)
        return Json("Login");

      await _postReactService.Create(viewModel);
      ViewBag.PostId = viewModel.PostId;
      return View(await _postReactService.GetPostReacts(viewModel.PostId));
    }
  }
}