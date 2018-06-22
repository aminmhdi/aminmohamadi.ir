using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MyWeb.ServiceLayer;
using MyWeb.ServiceLayer.Contracts;
using MyWeb.Utility;
using MyWeb.ViewModel.Category;
using MyWeb.ViewModel.Post;

namespace MyWeb.Areas.Management.Controllers
{
  public class PostController : Controller
  {
    #region Fields

    private readonly IPostService _postService;

    #endregion

    #region Constructor

    public PostController(IPostService postService)
    {
      _postService = postService;
    }

    #endregion

    #region List,ListAjax

    [Mvc5Authorize(AssignableToRolePermissions.CanAccessPostList)]
    public async Task<ActionResult> Index()
    {
      return View(await _postService.GetPagedListAsync(new PostSearchRequest()));
    }

    public async Task<ActionResult> ListAjax(PostSearchRequest search)
    {
      var posts = await _postService.GetPagedListAsync(search);
      var viewModel = new PostListViewModel()
      {
        Posts = posts.Posts,
        SearchRequest = search
      };
      return PartialView("_ListAjax", viewModel);
    }

    #endregion

    #region Create

    [Mvc5Authorize(AssignableToRolePermissions.CanCreatePost)]
    public ActionResult Create()
    {
      return View(_postService.GetForCreate());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Mvc5Authorize(AssignableToRolePermissions.CanCreatePost)]
    public async Task<ActionResult> Create(PostInsertViewModel viewModel)
    {
      if (ModelState.IsValid)
      {
        viewModel.CreatorId = long.Parse(User.Identity.GetUserId());
        await _postService.Create(viewModel);
        ViewBag.Message = "مخاطب جدید با موفقیت ثبت شد.";
        return RedirectToAction("Index");
      }
      ViewBag.Message = "ثبت انجام نشد.";
      return View(viewModel);
    }

    #endregion

    #region Detail

    [Mvc5Authorize(AssignableToRolePermissions.CanAccessPostList)]
    public async Task<ActionResult> Details(long? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      var postDetail = await _postService.GetDetails(id);
      if (postDetail == null)
      {
        return HttpNotFound();
      }
      return View(postDetail);
    }

    #endregion

    #region Delete

    [Mvc5Authorize(AssignableToRolePermissions.CanDeletePost)]
    public async Task<ActionResult> Delete(long? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      var postDetail = await _postService.GetDetails(id);
      if (postDetail == null)
      {
        return HttpNotFound();
      }
      return View(postDetail);
    }

    [HttpPost, ActionName("Delete")]
    [Mvc5Authorize(AssignableToRolePermissions.CanDeletePost)]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(long id)
    {
      await _postService.DeleteAsync(id);
      return RedirectToAction("Index");
    }

    #endregion

    #region Edit

    [Mvc5Authorize(AssignableToRolePermissions.CanEditPost)]
    public async Task<ActionResult> Edit(long? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      var postEdit = await _postService.GetForEdit(id);
      if (postEdit == null)
      {
        return HttpNotFound();
      }
      return View(postEdit);
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    [Mvc5Authorize(AssignableToRolePermissions.CanEditPost)]
    public async Task<ActionResult> Edit(PostEditViewModel viewModel)
    {
      if (ModelState.IsValid)
      {
        viewModel.ModifierId = long.Parse(User.Identity.GetUserId());
        await _postService.Edit(viewModel);
        return RedirectToAction("Index");
      }

      ViewBag.Message = "ثبت انجام نشد.";
      return View(viewModel);
    }

    #endregion

  }
}