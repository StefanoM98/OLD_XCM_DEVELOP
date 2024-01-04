using API_XCM.Code;
using API_XCM.Models.XCM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static API_XCM.FilterConfig;

namespace API_XCM.Controllers
{
    [ViewAuthFilter]
    public class RootController : Controller
    {
        // GET: Root
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            var test = AuthHelper.Register(model);
            return View();
        }
    }
}