using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using MyWeb.ServiceLayer;
using MyWeb.ServiceLayer.Contracts;
using MyWeb.Utility;

namespace MyWeb.Areas.Management.Controllers
{
  public class HomeController : Controller
  {
    // GET: Management/Home
    #region Fields

    private readonly IDashboardService _dashboardService;

    #endregion

    #region Constructor

    public HomeController(IDashboardService dashboardService)
    {
      _dashboardService = dashboardService;
    }

    #endregion

    #region Index

    [Mvc5Authorize(AssignableToRolePermissions.CanAccessDashboard)]
    public ActionResult Index()
    {
      return View();
    }

    #endregion

    [Mvc5Authorize(AssignableToRolePermissions.CanAccessDashboard)]
    [OutputCache(Duration = (60 * 3), VaryByParam = "none", Location = OutputCacheLocation.ServerAndClient)]
    public async Task<string> CountOfWebsiteViewForToDayInHours()
    {
      return await _dashboardService.CountOfWebsiteViewForToDayInHours();
    }

    [Mvc5Authorize(AssignableToRolePermissions.CanAccessDashboard)]
    [OutputCache(Duration = (60 * 60 * 24), VaryByParam = "none", Location = OutputCacheLocation.ServerAndClient)]
    public async Task<string> CountOfWebsiteViewIn30DaysAgo()
    {
      return await _dashboardService.CountOfWebsiteViewIn30DaysAgo();
    }

    [Mvc5Authorize(AssignableToRolePermissions.CanAccessDashboard)]
    [OutputCache(Duration = (60 * 60 * 24), VaryByParam = "none", Location = OutputCacheLocation.ServerAndClient)]
    public async Task<string> CountOfWebsiteViewIn12MonthsAgo()
    {
      return await _dashboardService.CountOfWebsiteViewIn12MonthsAgo();
    }

    [Mvc5Authorize(AssignableToRolePermissions.CanAccessDashboard)]
    [OutputCache(Duration = (60 * 60 * 24), VaryByParam = "none", Location = OutputCacheLocation.ServerAndClient)]
    public async Task<string> CountOfWebsiteViewIn7YearsAgo()
    {
      return await _dashboardService.CountOfWebsiteViewIn7YearsAgo();
    }

    [Mvc5Authorize(AssignableToRolePermissions.CanAccessDashboard)]
    public async Task<ActionResult> LastComments()
    {
      return View(await _dashboardService.LastComments());
    }

    [Mvc5Authorize(AssignableToRolePermissions.CanAccessDashboard)]
    public async Task<ActionResult> MostViewedPosts()
    {
      return View(await _dashboardService.MostViewedPosts());
    }

    [Mvc5Authorize(AssignableToRolePermissions.CanAccessDashboard)]
    [OutputCache(Duration = (60 * 60 * 24), VaryByParam = "none", Location = OutputCacheLocation.ServerAndClient)]
    public async Task<string> ViewersDevice()
    {
      return await _dashboardService.ViewersDevice();
    }

    public async Task<ActionResult> WebsiteStatistics()
    {
      return View(await _dashboardService.WebsiteStatistics());
    }

  }
}