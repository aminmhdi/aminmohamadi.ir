using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MyWeb.ServiceLayer;
using MyWeb.ServiceLayer.Contracts;
using MyWeb.Utility;
using MyWeb.ViewModel.Comment;

namespace MyWeb.Areas.Management.Controllers
{
    public class CommentController : Controller
    {
    #region Fields

    private readonly ICommentService _commentService;

    #endregion

    #region Constructor

    public CommentController(ICommentService commentService)
    {
      _commentService = commentService;
    }

    #endregion

    #region List,ListAjax

    [Mvc5Authorize(AssignableToRolePermissions.CanAccessCommentList)]
    public async Task<ActionResult> Index()
    {
      return View(await _commentService.GetPagedListAsync(new CommentSearchRequest()));
    }

    [AjaxOnly]
    public async Task<ActionResult> ListAjax(CommentSearchRequest search)
    {
      var comments = await _commentService.GetPagedListAsync(search);
      var viewModel = new CommentListViewModel
      {
        Comments = comments.Comments,
        SearchRequest = search
      };
      return PartialView("_ListAjax", viewModel);
    }

    #endregion

    #region Create

    [Mvc5Authorize(AssignableToRolePermissions.CanCreateComment)]
    public ActionResult Create()
    {
      return View(new CommentInsertViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Mvc5Authorize(AssignableToRolePermissions.CanCreateComment)]
    public async Task<ActionResult> Create(CommentInsertViewModel viewModel)
    {
      if (ModelState.IsValid)
      {
        viewModel.CreatorId = long.Parse(User.Identity.GetUserId());
        await _commentService.Create(viewModel);
        ViewBag.Message = "مخاطب جدید با موفقیت ثبت شد.";
        return RedirectToAction("Index");
      }
      ViewBag.Message = "ثبت انجام نشد.";
      return View(viewModel);
    }

    #endregion

    #region Detail

    [Mvc5Authorize(AssignableToRolePermissions.CanAccessCommentList)]
    public async Task<ActionResult> Details(long? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      var commentDetail = await _commentService.GetDetails(id);
      if (commentDetail == null)
      {
        return HttpNotFound();
      }
      return View(commentDetail);
    }

    #endregion

    #region Delete

    [Mvc5Authorize(AssignableToRolePermissions.CanDeleteComment)]
    public async Task<ActionResult> Delete(long? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      var commentDetail = await _commentService.GetDetails(id);
      if (commentDetail == null)
      {
        return HttpNotFound();
      }
      return View(commentDetail);
    }

    [HttpPost, ActionName("Delete")]
    [Mvc5Authorize(AssignableToRolePermissions.CanDeleteComment)]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(long id)
    {
      await _commentService.DeleteAsync(id);
      return RedirectToAction("Index");
    }

    #endregion

    #region Edit

    [Mvc5Authorize(AssignableToRolePermissions.CanEditComment)]
    public async Task<ActionResult> Edit(long? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      var commentEdit = await _commentService.GetForEdit(id);
      if (commentEdit == null)
      {
        return HttpNotFound();
      }
      return View(commentEdit);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Mvc5Authorize(AssignableToRolePermissions.CanEditComment)]
    public async Task<ActionResult> Edit(CommentEditViewModel viewModel)
    {
      if (ModelState.IsValid)
      {
        viewModel.ModifierId = long.Parse(User.Identity.GetUserId());
        await _commentService.Edit(viewModel);
        return RedirectToAction("Index");
      }

      ViewBag.Message = "ثبت انجام نشد.";
      return View(viewModel);
    }

    #endregion

    #region Active,Deactive

    [Mvc5Authorize(AssignableToRolePermissions.CanEditComment)]
    public async Task<ActionResult> Active(CommentSearchRequest search)
    {
      if (search.Id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }

      await _commentService.Active(search.Id, long.Parse(User.Identity.GetUserId()));
      return PartialView("_ListAjax", await _commentService.GetPagedListAsync(new CommentSearchRequest
      {
        PageIndex = search.PageIndex,
        Body = search.Body
      }));
    }

    #endregion
  }
}