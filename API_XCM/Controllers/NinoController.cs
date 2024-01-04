using API_XCM.Code;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using static API_XCM.FilterConfig;

namespace API_XCM.Controllers
{
    [AuthFilterNino]
    public class NinoController : Controller
    {
        // GET: Nino
        XCM xcm = new XCM();

        public ActionResult Index()
        {
            return View("Index", xcm.GetDocs());
        }

        [ValidateInput(false)]
        public ActionResult MasterGridPartial()
        {
            return PartialView("MasterGridPartial", xcm.GetDocs());
        }
        
        [ValidateInput(false)]
        public ActionResult DetailGridPartial(string CustomerID)
        {
            ViewData["AnaId"] = CustomerID;
            return PartialView("DetailGridPartial", XCM.GetResocontoDocumentiNonSpeditiDaIDGespe(CustomerID));
        }

        public void RefreshGridView()
        {
            xcm.RefreshModel();
        }

        public ActionResult Logout()
        {
            Session["nino"] = String.Empty;
            return RedirectToAction("Index", "Home");
        }
    }
}