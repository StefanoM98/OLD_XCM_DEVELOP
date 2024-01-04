using DevExpress.Web.Mvc;
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
    public class ApsController : Controller
    {
        XCM xcm = new XCM();


        public ActionResult Index()
        {
            return View();
        }


        //[ValidateInput(false)]
        //public ActionResult ApsGridViewPartial()
        //{
        //    return PartialView("_ApsGridViewPartial", xcm.GetDocumentsAPS());
        //}

        //[HttpPost, ValidateInput(false)]
        //public ActionResult ApsGridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] API_XCM.Models.XCM.ApsDafneResocontoModel item)
        //{
        //    var model = xcm.GetDocumentsAPS();
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var modelItem = model.FirstOrDefault(x => x.DocumentID == item.DocumentID);
        //            if(modelItem != null)
        //            {
        //                var bodyDaInviareAPIEspritec = new UpdateDocumentRowRootobject()
        //                {
        //                    row = new Row()
        //                    {
        //                        id = modelItem.RowID,
        //                        partNumber = modelItem.PartNumber,
        //                        qty = item.Quantita
        //                    }

        //                };

        //                var response = xcm.UpdateDocumentRowESPRITEC(bodyDaInviareAPIEspritec);
        //                if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //                {
        //                    modelItem.Quantita = item.Quantita;
        //                }
        //                else
        //                {
        //                    ViewData["EditError"] = response.ErrorMessage;
        //                }
        //            }
        //            else
        //            {
        //                ViewData["EditError"] = $"Impossibile aggiornare la quantità. Il documento {item.DocumentID} non è stato trovato, o ha cambiato stato";

        //            }

        //        }
        //        catch (Exception e)
        //        {
        //            ViewData["EditError"] = e.Message;
        //        }
        //    }
        //    else
        //    {
        //        ViewData["EditError"] = "Prefavore, correggi tutti gli errori";

        //    }
        //    return PartialView("_ApsGridViewPartial", model);
        //}

        public ActionResult Logout()
        {
            Session["isAuthenticated"] = String.Empty;
            return RedirectToAction("Index", "Home");
        }
    }
}


