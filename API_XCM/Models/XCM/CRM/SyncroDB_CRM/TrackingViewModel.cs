using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_XCM.Models.XCM.CRM.SyncroDB_CRM
{
    public class TrackingViewModel
    {
        public DateTime Tracking_date { get; set; }
        public string Tracking_statusDes { get; set; }
        public string Tracking_info { get; set; }
    }
}