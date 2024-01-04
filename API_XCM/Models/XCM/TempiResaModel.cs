using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_XCM.Models.XCM
{
    public class TempiResaModel
    {
        public string DocDate { get; set; }
        public string DocNumber { get; set; }
        public string ExternalRef { get; set; }
        public string StatusDes { get; set; }
        public string DataConsegna { get; set; }
        public string TempiResa { get; set; }
        public string UnloadDes { get; set; }
        public string UnloadAddress { get; set; }
        public string UnloadZipCode { get; set; }
        public string UnloadDistrict { get; set; }
        public string UnloadLocation { get; set; }
        public int Colli { get; set; }
        public int Bancali { get; set; }
        public decimal Peso { get; set; }

        public string Corrispondente { get; set; }
        public string Linea { get; set; }
        public string RiferimentoUnitex { get; set; }
        public string RiferimentoEsternoUnitex { get; set; }

    }
}