using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace MyWeb.Controllers
{
  public class FileController : Controller
  {
    // GET: Files
    public ActionResult Index(string folder, string file)
    {
      var fileLocation = Server.MapPath("~/Files/" + folder + "/" + file);

      if (!System.IO.File.Exists(fileLocation))
        return File(Server.MapPath("~/Content/Images/user.svg"), "image/svg+xml");

      var fileBytes = System.IO.File.ReadAllBytes(fileLocation);
      return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, file);
    }
  }
}