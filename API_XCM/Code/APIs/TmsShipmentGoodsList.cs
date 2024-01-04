using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace API_XCM.Code.APIs
{
    public class TmsShipmentGoodsList
    {
        public TmsShipmentGoodsListResult result { get; set; }
        public Good[] goods { get; set; }
    }

    public class TmsShipmentGoodsListResult
    {
        public object[] messages { get; set; }
        public string info { get; set; }
        public int maxPages { get; set; }
        public bool status { get; set; }
    }

    public class Good
    {
        public int id { get; set; }
        public int shipID { get; set; }
        public int loadStopID { get; set; }
        public int unLoadStopID { get; set; }
        public string type { get; set; }
        public string description { get; set; }
        public string holderID { get; set; }
        public string packsTypeID { get; set; }
        public string packsTypeDes { get; set; }
        public int packs { get; set; }
        public int floorPallet { get; set; }
        public int totalPallet { get; set; }
        public decimal netWeight { get; set; }
        public decimal grossWeight { get; set; }
        public decimal cube { get; set; }
        public decimal meters { get; set; }
        public decimal height { get; set; }
        public decimal width { get; set; }
        public decimal deep { get; set; }
        public decimal seat { get; set; }
        public string containerNo { get; set; }
    }

}