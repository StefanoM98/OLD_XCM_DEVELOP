using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_XCM.Code.APIs
{
    public class TmsShipmentTrackingList
    {
        public TmsShipmentTrackingListResult result { get; set; }
        public TmsShipmentTrackingListEvent[] events { get; set; }
    }

    public class TmsShipmentTrackingListResult
    {
        public object[] messages { get; set; }
        public string info { get; set; }
        public int maxPages { get; set; }
        public bool status { get; set; }
    }

    public class TmsShipmentTrackingListEvent
    {
        public int id { get; set; }
        public int shipID { get; set; }
        public int stopID { get; set; }
        public string stopType { get; set; }
        public string stopDescription { get; set; }
        public string stopAddress { get; set; }
        public string stopZipCode { get; set; }
        public string stopLocation { get; set; }
        public string stopDistrict { get; set; }
        public string stopRegion { get; set; }
        public string stopCountry { get; set; }
        public int tripID { get; set; }
        public string agencycode { get; set; }
        public int statusID { get; set; }
        public string statusType { get; set; }
        public string statusDes { get; set; }
        public string statusColor { get; set; }
        public int webStatusID { get; set; }
        public string webStatusDes { get; set; }
        public string webStatusColor { get; set; }
        public DateTime timeStamp { get; set; }
        public string info { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }
        public string locationInfo { get; set; }
        public string signature { get; set; }
    }
}