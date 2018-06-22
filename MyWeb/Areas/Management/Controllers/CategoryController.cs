using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MyWeb.ServiceLayer;
using MyWeb.ServiceLayer.Contracts;
using MyWeb.Utility;
using MyWeb.ViewModel.Category;

namespace MyWeb.Areas.Management.Controllers
{
  public class CategoryController : Controller
  {
    #region Fields

    private readonly ICategoryService _categoryService;

    #endregion

    #region Constructor

    public CategoryController(ICategoryService categoryService)
    {
      _categoryService = categoryService;
    }

    #endregion

    #region List,ListAjax

    [Mvc5Authorize(AssignableToRolePermissions.CanAccessCategoryList)]
    public async Task<ActionResult> Index()
    {
      return View(await _categoryService.GetPagedListAsync(new CategorySearchRequest()));
    }

    [AjaxOnly]
    public async Task<ActionResult> ListAjax(CategorySearchRequest search)
    {
      var categories = await _categoryService.GetPagedListAsync(search);
      var viewModel = new CategoryListViewModel
      {
        Categories = categories.Categories,
        SearchRequest = search
      };
      return PartialView("_ListAjax", viewModel);
    }

    #endregion

    #region Create

    [Mvc5Authorize(AssignableToRolePermissions.CanCreateCategory)]
    public ActionResult Create()
    {
      return View(new CategoryInsertViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Mvc5Authorize(AssignableToRolePermissions.CanCreateCategory)]
    public async Task<ActionResult> Create(CategoryInsertViewModel viewModel)
    {
      if (ModelState.IsValid)
      {
        viewModel.CreatorId = long.Parse(User.Identity.GetUserId());
        await _categoryService.Create(viewModel);
        ViewBag.Message = "مخاطب جدید با موفقیت ثبت شد.";
        return RedirectToAction("Index");
      }
      ViewBag.Message = "ثبت انجام نشد.";
      return View(viewModel);
    }

    #endregion

    #region Detail

    [Mvc5Authorize(AssignableToRolePermissions.CanAccessCategoryList)]
    public async Task<ActionResult> Details(long? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      var categoryDetail = await _categoryService.GetDetails(id);
      if (categoryDetail == null)
      {
        return HttpNotFound();
      }
      return View(categoryDetail);
    }

    #endregion

    #region Delete

    [Mvc5Authorize(AssignableToRolePermissions.CanDeleteCategory)]
    public async Task<ActionResult> Delete(long? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      var categoryDetail = await _categoryService.GetDetails(id);
      if (categoryDetail == null)
      {
        return HttpNotFound();
      }
      return View(categoryDetail);
    }

    [HttpPost, ActionName("Delete")]
    [Mvc5Authorize(AssignableToRolePermissions.CanDeleteCategory)]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(long id)
    {
      await _categoryService.DeleteAsync(id);
      return RedirectToAction("Index");
    }

    #endregion

    #region Edit

    [Mvc5Authorize(AssignableToRolePermissions.CanEditCategory)]
    public async Task<ActionResult> Edit(long? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      var categoryEdit = await _categoryService.GetForEdit(id);
      if (categoryEdit == null)
      {
        return HttpNotFound();
      }
      return View(categoryEdit);
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    [Mvc5Authorize(AssignableToRolePermissions.CanEditCategory)]
    public async Task<ActionResult> Edit(CategoryEditViewModel viewModel)
    {
      if (ModelState.IsValid)
      {
        viewModel.ModifierId = long.Parse(User.Identity.GetUserId());
        await _categoryService.Edit(viewModel);
        return RedirectToAction("Index");
      }

      ViewBag.Message = "ثبت انجام نشد.";
      return View(viewModel);
    }

    #endregion

  }
}