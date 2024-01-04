using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_XCM.Code.APIs
{
    public class TmsShipmentTrackingNewResponse
    {

        public TmsShipmentTrackingNewResponseResult result { get; set; }
        public int id { get; set; }


    }

    public class TmsShipmentTrackingNewResponseResult
    {
        public object[] messages { get; set; }
        public string info { get; set; }
        public int maxPages { get; set; }
        public bool status { get; set; }
    }
}