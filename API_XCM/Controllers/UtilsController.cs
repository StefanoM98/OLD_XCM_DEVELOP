using API_XCM.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace API_XCM.Controllers
{
    public class UtilsController : Controller
    {
        // http://185.30.181.192:8092/utils/gettempiresa?token=!ebhdgGJTR%-jksjUT&startdate=10-01-2022&customerID=00015
        [HttpGet]
        public Object GetTempiResa(string token, string startDate, string customerID)
        {
            if(token != "!ebhdgGJTR%-jksjUT")
            {
                return null;
            }
            return File(XCM.GetTempiResa(startDate, customerID), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"TempiResa_{customerID}_{DateTime.Now.Ticks}.xlsx");
        }


        public void TrackingNewUnitex(string fileName, string fornitore, string startDate)
        {
            fileName = @"C:\UNITEX\Esiti-CDL-2511.csv";
            fornitore = "CDL";
            startDate = "09-01-2022";
            UNITEX.TrackingNewUNITEX(fileName, fornitore, startDate);
        }
    }
}