using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using MyWeb.ServiceLayer.Contracts;
using MyWeb.Utility;
using MyWeb.ViewModel.Post;

namespace MyWeb.Controllers
{
    public class CategoryController : Controller
    {
        #region Fields

        private readonly ICategoryService _categoryService;
        private readonly IPostService _postService;

        #endregion

        #region Constructor

        public CategoryController(ICategoryService categoryService, IPostService postService)
        {
            _categoryService = categoryService;
            _postService = postService;
        }

        #endregion
        // GET: Category
        public async Task<ActionResult> Index(long id, string title, int? page = 1)
        {
            var idAndTitle = await _categoryService.GetForMainIdAndTitle(id);
            if (idAndTitle == null)
                throw new HttpException(404, "Page not Found");

            if (title != idAndTitle.Title.ToUrl())
            {
                return page == 1 ? 
                    Redirect("/Category/" + idAndTitle.Id + "/" + idAndTitle.Title.ToUrl()) : 
                    Redirect("/Category/" + idAndTitle.Id + "/" + idAndTitle.Title.ToUrl() + "/" + page);
            }

            var posts = await _postService.GetForMainPagePagedListForAsync(new PostSearchRequest
            {
                CategoryId = id,
                PageIndex = page.Value
            });

            if (!string.IsNullOrEmpty(posts.SearchRequest.CategoryTitle))
                return View(posts);
            throw new HttpException(404, "Page not Found");
        }

        [OutputCache(Duration = 86400, VaryByParam = "*")]
        public ActionResult CategoryMenu()
        {
            return View(_categoryService.GetAllCategoriesForMenu());
        }
    }
}