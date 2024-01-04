using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace UnitexFSC.Code.APIs
{
    public class TmsShipmentParcelList
    {
        public TmsShipmentParcelListResult result { get; set; }
        public Parcel[] parcel { get; set; }
    }

    public class TmsShipmentParcelListResult
    {
        public object[] messages { get; set; }
        public string info { get; set; }
        public int maxPages { get; set; }
        public bool status { get; set; }
    }

    public class Parcel
    {
        public int id { get; set; }
        public int shipID { get; set; }
        public int stopID { get; set; }
        public int goodsID { get; set; }
        public string siteID { get; set; }
        public string barcode { get; set; }
        public string barcodeExt { get; set; }
        public string barcodeMaster { get; set; }
        public int statusID { get; set; }
        public string statusDes { get; set; }
        public decimal qty { get; set; }
        public decimal netWeight { get; set; }
        public decimal grossWeight { get; set; }
        public decimal cube { get; set; }
        public decimal width { get; set; }
        public decimal height { get; set; }
        public decimal deep { get; set; }
    }

}