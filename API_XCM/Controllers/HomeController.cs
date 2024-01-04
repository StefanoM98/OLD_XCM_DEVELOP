using API_XCM.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static API_XCM.FilterConfig;

namespace API_XCM.Controllers
{
    [AuthFilter]
    public class HomeController : Controller
    {
        // /Home/index?usr={G>E<3.?4=ks:xm-&pwd=mHL7^nK~6[c8'Py8
        public ActionResult Index(string usr, string pwd)
        {
            ViewBag.Title = "XCM Healthcare";

            return View();
            
        }
    }
}
