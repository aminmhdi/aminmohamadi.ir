using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MyWeb.ServiceLayer.Contracts;
using MyWeb.Utility;
using MyWeb.ViewModel.WebPostView;
using MyWeb.ViewModel.WebView;

namespace MyWeb.Controllers
{
    public class StatisticsController : Controller
    {
        #region Fields

        private readonly IWebViewService _webViewService;
        private readonly IWebPostViewService _webPostViewService;

        #endregion

        #region Constructor

        public StatisticsController(IWebViewService webViewService, IWebPostViewService webPostViewService)
        {
            _webViewService = webViewService;
            _webPostViewService = webPostViewService;
        }

        #endregion

        // GET: Statistics
        [HttpPost]
        public async Task Index(long? id)
        {
            try
            {
                if (!id.HasValue)
                    await _webViewService.Create(new WebViewInsertViewModel
                    {
                        Ip = Network.GetClientIp(),
                        Session = Network.GetSessionId(),
                        UserId = User.Identity.GetUserId() != null ? long.Parse(User.Identity.GetUserId()) : (long?)null,
                        BrowserName = Request.Browser.Browser,
                        BrowserVersion = Request.Browser.Version,
                        Platfrom = Request.Browser.Platform,
                        Device = Request.Browser.IsMobileDevice ? "Mobile" : "Desktop",
                        MoreInfo = Request.UserAgent
                    });

                else
                    await _webPostViewService.Create(new WebPostViewInsertViewModel
                    {
                        Ip = Network.GetClientIp(),
                        Session = Network.GetSessionId(),
                        UserId = User.Identity.GetUserId() != null ? long.Parse(User.Identity.GetUserId()) : (long?)null,
                        PostId = id.Value,
                        BrowserName = Request.Browser.Browser,
                        BrowserVersion = Request.Browser.Version,
                        Platfrom = Request.Browser.Platform,
                        Device = Request.Browser.IsMobileDevice ? "Mobile" : "Desktop",
                        MoreInfo = Request.UserAgent
                    });

            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}