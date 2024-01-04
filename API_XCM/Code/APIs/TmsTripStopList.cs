using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace API_XCM.Code.APIs
{
    public class TmsTripStopList
    {
        public ResulTmsTripStopListt result { get; set; }
        public Stop[] stops { get; set; }
    }

    public class ResulTmsTripStopListt
    {
        public object[] messages { get; set; }
        public string info { get; set; }
        public int maxPages { get; set; }
        public bool status { get; set; }
    }

    public class Stop
    {
        public int id { get; set; }
        public int shipID { get; set; }
        public int stopID { get; set; }
        public string type { get; set; }
        public object date { get; set; }
        public string shipDocNumber { get; set; }
        public string shipInsideRef { get; set; }
        public string shipExternRef { get; set; }
        public int locationID { get; set; }
        public string description { get; set; }
        public string address { get; set; }
        public string zipCode { get; set; }
        public string location { get; set; }
        public string district { get; set; }
        public string region { get; set; }
        public string country { get; set; }
        public decimal longitude { get; set; }
        public decimal latitude { get; set; }
        public string contactName { get; set; }
        public string contactPhone { get; set; }
        public string contactPhone1 { get; set; }
        public string contactMail { get; set; }
        public string contactMail1 { get; set; }
        public int packs { get; set; }
        public int floorPallet { get; set; }
        public int totalPallet { get; set; }
        public decimal netWeight { get; set; }
        public decimal grossWeight { get; set; }
        public decimal cube { get; set; }
        public decimal meters { get; set; }
        public string driver1ID { get; set; }
        public string driver1Des { get; set; }
        public string driver2ID { get; set; }
        public string driver2Des { get; set; }
        public string vehicleID { get; set; }
        public string towID { get; set; }
    }

}
