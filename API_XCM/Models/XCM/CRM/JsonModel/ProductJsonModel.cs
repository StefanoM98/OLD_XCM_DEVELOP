using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_XCM.Models.XCM.CRM.JsonModel
{
    public class ProductJsonModel
    {
        public long ID_ANAGRAFICA_PRODOTTO { get; set; }
        public string CODICE_PRODOTTO { get; set; }
        public string DESCRIZIONE_PRODOTTO { get; set; }
        public decimal PREZZO_UNITARIO { get; set; }
        public DateTime DATA_CREAZIONE { get; set; }
        public DateTime DATA_ULTIMA_MODIFICA { get; set; }
        public string GESPE_CUSTOMERID { get; set; }
        public int QUANTITA { get; set; }
        public decimal SCONTO { get; set; }
        public decimal IVA { get; set; }
    }
}