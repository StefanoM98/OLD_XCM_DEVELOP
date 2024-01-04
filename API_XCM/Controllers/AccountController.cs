using API_XCM.Code;
using API_XCM.Models;
using API_XCM.Models.XCM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace API_XCM.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                if(model.Username == "info@xcmhealthcare.com")
                {
                    if(AuthHelper.SignIn(model.Username, model.Password))
                    {
                        return RedirectToAction("Index", "Root");
                    }
                    else
                    {
                        ViewBag.GeneralError = "Accesso fallito";
                        return View(model);
                    }
                }
                else
                {
                    if (AuthHelper.Login(model.Username, model.Password))
                    {
                        if (model.Username == "aps")
                        {
                            Session["isAuthenticated"] = "xwHD14";
                            return RedirectToAction("Index", "Aps");

                        }
                        else if (model.Username == "g.colella")
                        {
                            Session["nino"] = "mkTY14";
                            return RedirectToAction("Index", "Nino");
                        }
                        else
                        {
                            ViewBag.GeneralError = "Accesso fallito";
                            return View(model);
                        }

                    }
                    else
                    {
                        ViewBag.GeneralError = "Accesso fallito";
                        return View(model);
                    }
                }
               

            }
            else
            {
                return View(model);
            }

        }

        public ActionResult SignOut()
        {
            AuthHelper.SignOut();
            return RedirectToAction("Index", "Home");
        }


    }
}