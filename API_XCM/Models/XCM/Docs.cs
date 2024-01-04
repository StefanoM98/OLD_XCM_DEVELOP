using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_XCM.Models.XCM
{
    public class Docs
    {
        public string Mandante { get; set; }
        public string GespeID { get; set; }
        public int NuoviOrdiniGiornalieri { get; set; }
        public int Evasi { get; set; }
        public int NonEvasi { get; set; }

        public int NonEvasiPrec { get; set; }
        public int Spediti { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Decimal Quantity { get; set; }
    }
}