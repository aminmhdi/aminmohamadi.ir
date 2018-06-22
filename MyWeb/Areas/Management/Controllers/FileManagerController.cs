using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyWeb.Areas.Management.Controllers
{
    public class FileManagerController : Controller
    {
        // GET: Management/FileManager
        public ActionResult Index()
        {
            return View();
        }
    }
}