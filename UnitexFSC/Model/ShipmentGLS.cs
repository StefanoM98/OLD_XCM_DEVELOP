using DevExpress.XtraPrinting.Native;
using DevExpress.XtraSpreadsheet.Import.Xls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace API_XCM.Models.UNITEX
{
    public class ShipmentGLS
    {
        public string DocNum { get; set; } 
        public string DataDoc { get; set; }
        public string ExternalRef { get; set; }
        public string UnloadDes { get; set; }
        public string UnloadAddress { get; set; }
        public string UnloadZipCode{ get; set; }
        public string UnloadLocation { get; set; }
        public string UnloadDistrict{ get; set; }
        public string UnloadCountry{ get; set; }
        public int Packs { get; set;}
        public decimal GrossWeight { get; set; }
        public string Info { get; set; }
        public string TripNum { get; set; }

        public decimal Contrassegno { get; set; }
        public int Sprinter = 28;
        public string Barcodes { get; set; }

    }
}