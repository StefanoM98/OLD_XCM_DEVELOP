using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VivisolPalletXCM.Models;
using RestSharp;
using ZebraUtils;


namespace VivisolPalletXCM.Controllers
{
    public class HomeController : Controller
    {


        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            var model = new FormModel();
            if(Session["numeroConsegna"] != null)
            {
                model.numeroConsegna = Session["numeroConsegna"].ToString();

            }
            else
            {
                model.numeroConsegna = "1";
                Session["numeroConsegna"] = model.numeroConsegna;
            }
            return View(model);
        }

        //// POST: /
        //[HttpPost]
        //[AllowAnonymous]
        //public ActionResult Index(FormModel model)
        //{
        //    var idDocumento = model.idDocumento;

        //    var intIdDoc = int.Parse(idDocumento);

        //    var doc = new GnXcmEntities().uvwWmsDocument.FirstOrDefault(x => x.uniq == intIdDoc);
        //    var orm = doc.DocNum;
        //    var wmsDocumentOrm = orm.Split('/')[0];

        //    var suffisso = $"XCM{DateTime.Now.ToString("yy")}";


        //    while (wmsDocumentOrm.Length < 7)
        //    {
        //        wmsDocumentOrm = wmsDocumentOrm.Insert(0, "0");
        //    }
        //    wmsDocumentOrm = $"{suffisso}{wmsDocumentOrm}";

        //    var zebraUtils = new ZPLLibUtilsClass()
        //    {
        //        _ipAddress = "192.168.2.201",
        //        _port = 9100
        //    };

        //    try
        //    {
        //        zebraUtils.stampaBC128VivisolNew(wmsDocumentOrm, doc.UnloadName, model.numeroConsegna.ToString());
        //    }
        //    catch (Exception ee)
        //    {
        //        SetErrorText(ee.Message);
        //        ModelState.AddModelError("", ViewBag.GeneralError);
        //        return View(model);
        //    }

        //    ViewBag.Status = $"OK: Stampa inviata per il Documento n° [{model.idDocumento}] ORM n° [{orm}]";
        //    ModelState.AddModelError("", ViewBag.Status);
        //    model.idDocumento = String.Empty;
        //    var incremento = int.Parse(model.numeroConsegna);
        //    incremento++;
        //    model.numeroConsegna = incremento.ToString();
            

        //    return View(model);

        //}

        // POST: /
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(string idDocumento, string numeroConsegna)
        {
            var model = new FormModel();

            model.idDocumento = idDocumento;
            model.numeroConsegna = numeroConsegna;

            var intIdDoc = int.Parse(idDocumento);

            var doc = new GnXcmEntities().uvwWmsDocument.FirstOrDefault(x => x.uniq == intIdDoc);
            var orm = doc.DocNum;
            var wmsDocumentOrm = orm.Split('/')[0];

            var suffisso = $"XCM{DateTime.Now.ToString("yy")}";


            while (wmsDocumentOrm.Length < 7)
            {
                wmsDocumentOrm = wmsDocumentOrm.Insert(0, "0");
            }
            wmsDocumentOrm = $"{suffisso}{wmsDocumentOrm}";

            var zebraUtils = new ZPLLibUtilsClass()
            {
                _ipAddress = "192.168.2.201",
                _port = 9100
            };

            try
            {
                zebraUtils.stampaBC128VivisolNew(wmsDocumentOrm, doc.UnloadName, model.numeroConsegna);
            }
            catch (Exception ee)
            {
                SetErrorText(ee.Message);
                ModelState.AddModelError("", ViewBag.GeneralError);
                return View(model);
            }

            ViewBag.Status = $"OK: Stampa inviata per il Documento n° [{model.idDocumento}] ORM n° [{orm}]";
            ModelState.AddModelError("", ViewBag.Status);
            model.idDocumento = String.Empty;
            var incremento = int.Parse(model.numeroConsegna);
            incremento++;
            model.numeroConsegna = incremento.ToString();
            Session["numeroConsegna"] = model.numeroConsegna;

            return View(model);
        }
        protected void SetErrorText(string message)
        {
            ViewBag.GeneralError = message;
        }

    }
}