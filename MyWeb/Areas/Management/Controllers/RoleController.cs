using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using MyWeb.DataLayer.Context;
using MyWeb.ServiceLayer;
using MyWeb.ServiceLayer.Contracts;
using MyWeb.Utility;
using MyWeb.ViewModel.Role;
using WebGrease.Css.Extensions;

namespace MyWeb.Areas.Management.Controllers
{
    public class RoleController : Controller
    {
        // GET: Role
        #region Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly IApplicationRoleManager _roleManager;

        #endregion

        #region Constructor

        public RoleController(IUnitOfWork unitOfWork, IApplicationRoleManager roleManager)
        {
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
        }

        #endregion

        #region ListAjax,List
        [HttpGet]
        [Mvc5Authorize(AssignableToRolePermissions.CanViewRolesList)]
        [DisplayName("مشاهده لیست گروه های کاربری")]
        public virtual ActionResult Index()
        {
            return View();
        }

        //[CheckReferrer]
        [Mvc5Authorize(AssignableToRolePermissions.CanViewRolesList)]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public virtual ActionResult ListAjax(string term = "", int page = 1)
        {
            var roles = _roleManager.GetPageList(out var total, term, page, 5);
            ViewBag.TotalRoles = total;
            ViewBag.PageNumber = page;
            return PartialView("_ListAjax", roles);
        }
        #endregion

        #region Create

        [HttpGet]
        [Mvc5Authorize(AssignableToRolePermissions.CanCreateRole)]
        [DisplayName("ثبت گروه کاربری جدید")]
        public virtual ActionResult Create()
        {
            var viewModel = new AddRoleViewModel
            {
                IsActive = true
            };
            PopulatePermissions();
            return View(viewModel);
        }
        [HttpPost]
        [Mvc5Authorize(AssignableToRolePermissions.CanCreateRole)]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Create(AddRoleViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "لطفا فیلد های مورد نظر را با دقت وارد کنید";
                PopulatePermissions(viewModel.PermissionNames);
                return View(viewModel);
            }
            if (!await _roleManager.AddRole(viewModel))
            {
                ViewBag.ErrorMessage = "لطفا برای گروه کاربری مورد نظر ، دسترسی تعیین کنید";
                PopulatePermissions();
                return View(viewModel);
            }

            await _unitOfWork.SaveChangesAsync();
            ViewBag.Message = "عملیات ثبت گروه کاربری جدید با موفقیت انجام شد";
            return RedirectToAction("Index", "Role");
        }

        #endregion

        #region Edit
        [HttpGet]
        [DisplayName("ویرایش گروه کاربری")]
        [Mvc5Authorize(AssignableToRolePermissions.CanEditRole)]
        //[Route("Edit/{id}")]
        public virtual async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var viewModel = await _roleManager.GetRoleByPermissionsAsync(id.Value);
            if (viewModel == null)
                return HttpNotFound();
            PopulatePermissions(viewModel.PermissionNames);
            return View(viewModel);
        }

        //[Route("Edit/{id}")]
        [Mvc5Authorize(AssignableToRolePermissions.CanEditRole)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Edit(EditRoleViewModel viewModel)
        {
            if (_roleManager.ChechForExisByName(viewModel.Name, viewModel.Id))
                this.AddErrors("Name", "این گروه  قبلا در سیستم ثبت شده است");

            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "لطفا فیلد های مورد نظر را با دقت وارد کنید";
                PopulatePermissions(viewModel.PermissionNames);
                return View(viewModel);
            }

            var dbRole = await _roleManager.FindByIdAsync(viewModel.Id);
            if (dbRole == null)
                return HttpNotFound();

            if (!await _roleManager.EditRole(viewModel))
            {
                ViewBag.ErrorMessage = "لطفا برای گروه کاربری مورد نظر ، دسترسی تعیین کنید";
                PopulatePermissions();
                return View(viewModel);
            }
            await _unitOfWork.SaveChangesAsync();
            ViewBag.Message = "عملیات ویرایش گروه کاربری  با موفقیت انجام شد";
            return RedirectToAction("Index", "Role");
        }

        #endregion

        #region Delete

        [HttpPost]
        //[Route("Delete/{id}")]
        [ValidateAntiForgeryToken]
        [Mvc5Authorize(AssignableToRolePermissions.CanDeleteRole)]
        [DisplayName("حذف گروه کاربری")]
        public virtual async Task<ActionResult> Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (await _roleManager.CheckRoleIsSystemRoleAsync(id.Value))
            {
                ViewBag.ErrorMessage = "این گروه کاربری سیستمی است و حذف آن باعث اختلال در سیستم خواهد شد";
                return RedirectToAction("Index", "Role");
            }
            await _roleManager.RemoveById(id.Value);
            ViewBag.Message = "گروه مورد نظر با موفقیت حذف شد";
            return RedirectToAction("Index", "Role");
        }

        #endregion

        #region RemoteValidation

        [HttpPost]
        [AjaxOnly]
        [Mvc5Authorize(AssignableToRolePermissions.CanCreateRole, AssignableToRolePermissions.CanEditRole)]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public virtual JsonResult RoleNameExist(string name, int? id)
        {
            return _roleManager.ChechForExisByName(name, id) ? Json(false) : Json(true);
        }

        #endregion

        #region Private
        [NonAction]
        private void PopulatePermissions(params string[] selectedpermissions)
        {
            var permissions = AssignableToRolePermissions.GetAsSelectListItems();

            if (selectedpermissions != null)
            {
                permissions.ForEach(
                    a => a.Selected = selectedpermissions.Any(s => s == a.Value));
            }

            ViewBag.Permissions = permissions;
        }

        #endregion
    }
}