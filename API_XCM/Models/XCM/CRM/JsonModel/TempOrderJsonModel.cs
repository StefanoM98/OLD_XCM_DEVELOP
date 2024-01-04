using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_XCM.Models.XCM.CRM.JsonModel
{
    public class TempOrderJsonModel
    {
        public long OrderID { get; set; }
        public string CustomerID { get; set; }
        public string AgentID { get; set; }
        public long UnloadLocationID { get; set; }
        public long? ConsigneeLocationID { get; set; }
        public string OrderType { get; set; }
        public string OrderReference { get; set; }
        public DateTime OrderReferenceDate { get; set; }
        public string XCMNote { get; set; }
        public string DeliveryNote { get; set; }

        public bool OrderConfirmed { get; set; }
        public bool OrderSended { get; set; }
    }
}