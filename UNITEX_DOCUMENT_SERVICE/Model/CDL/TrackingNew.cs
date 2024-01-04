
using System;

namespace UNITEX_DOCUMENT_SERVICE.Model.CDL
{
    public class TrackingNew
    {
        public int shipID { get; set; }
        public int stopID { get; set; }
        public int statusID { get; set; }
        public DateTime timeStamp { get; set; }
        public string info { get; set; }
        public string signature { get; set; }
        public decimal longitude { get; set; }
        public decimal latitude { get; set; }
        public string locationInfo { get; set; }
    }
}