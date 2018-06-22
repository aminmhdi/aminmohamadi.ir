using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CaptchaMvc.Attributes;
using Microsoft.AspNet.Identity;
using MyWeb.ServiceLayer.Contracts;
using MyWeb.Utility;
using MyWeb.ViewModel.Comment;

namespace MyWeb.Controllers
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

    // GET: Comment
    [OutputCache(Duration = 86400, VaryByParam = "*")]
    public ActionResult Index()
    {
      return View(_commentService.GetCommentForMainPagedList(new CommentSearchRequest
      {
        IsActive = ActiveStatus.Enable
      }));
    }

    [HttpGet]
    public ActionResult AddComment(long id)
    {
      return PartialView(_commentService.GetForCreate(id));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CaptchaVerify("تصویر امنیتی را درست وارد کنید")]
    public async Task<ActionResult> AddComment(CommentInsertViewModel comment)
    {
      comment.CreatorId = long.Parse(User.Identity.GetUserId());
      comment.Ip = Network.GetClientIp();
      await _commentService.Create(comment);
      return PartialView("AddCommentSuccessfully");
    }
  }
}