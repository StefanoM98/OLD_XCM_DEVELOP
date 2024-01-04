using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_XCM.Models.XCM.CRM.JsonModel
{
    public class TrackingJsonModel
    {
        public long ID_TRACKING { get; set; }
        public long Tracking_ShipmentID { get; set; }
        public System.DateTime Tracking_Data { get; set; }
        public int Tracking_StatusID { get; set; }
        public string Tracking_StatusDes { get; set; }
    }
}