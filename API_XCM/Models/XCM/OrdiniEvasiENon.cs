using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_XCM.Models.XCM
{
    public class OrdiniEvasiENon
    {
        public string Mandante { get; set; }
        public int TotaleOrdiniGiornalieri { get; set; }
        public int OrdiniEvasi { get; set; }
        public int NonEvasi { get; set; }

    }
}