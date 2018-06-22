using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MyWeb.ServiceLayer.Contracts;
using MyWeb.Utility;
using MyWeb.ViewModel.Post;
using MyWeb.ViewModel.WebPostView;
using MyWeb.ViewModel.WebView;

namespace MyWeb.Controllers
{
    public class PostController : Controller
    {
        // GET: Post

        #region Fields

        private readonly IPostService _postService;

        #endregion

        #region Constructor

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        #endregion

        public async Task<ActionResult> Index(long? id, string title)
        {
            if (id == null)
                return View("Index", await _postService.GetForMainPagePagedListForAsync(new PostSearchRequest()));


            var idAndTitle = await _postService.GetForMainIdAndTitle(id);

            if (idAndTitle == null)
                throw new HttpException(404, "Page not Found");
            if (title != idAndTitle.Title.ToUrl())
                return Redirect("/Post/" + idAndTitle.Id + "/" + idAndTitle.Title.ToUrl());


            var postDetail = await _postService.GetForMainPageDetails(id);
            if (postDetail == null)
                throw new HttpException(404, "Page not Found");

            return View("Detail", postDetail);
        }
    }
}