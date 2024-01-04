using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNITEX_DOCUMENT_SERVICE.Model.Espritec_API.UNITEX
{

    public class TmsShipmentList
    {
        public TmsShipmentListResult result { get; set; }
        public Shipment[] shipments { get; set; }
    }

    public class TmsShipmentListResult
    {
        public object[] messages { get; set; }
        public string info { get; set; }
        public int maxPages { get; set; }
        public bool status { get; set; }
    }

    public class Shipment
    {
        public int id { get; set; }
        public string ownerAgency { get; set; }
        public string docNumber { get; set; }
        public DateTime docDate { get; set; }
        public int statusId { get; set; }
        public string statusType { get; set; }
        public string statusDes { get; set; }
        public int webStatusId { get; set; }
        public string webStatusType { get; set; }
        public int webOrderID { get; set; }
        public string webOrderNumber { get; set; }
        public string insideRef { get; set; }
        public string externRef { get; set; }
        public string serviceType { get; set; }
        public string transportType { get; set; }
        public string customerID { get; set; }
        public string customerDes { get; set; }
        public string pickupSupplierID { get; set; }
        public string pickupSupplierDes { get; set; }
        public string deliverySupplierID { get; set; }
        public string deliverySupplierDes { get; set; }
        public Nullable<DateTime> pickupDateTime { get; set; }
        public object deliveryDateTime { get; set; }
        public string senderID { get; set; }
        public string senderDes { get; set; }
        public string consigneeID { get; set; }
        public string consigneeDes { get; set; }
        public int firstStopID { get; set; }
        public string firstStopDes { get; set; }
        public int lastStopID { get; set; }
        public string lastStopDes { get; set; }
        public string lastStopAddress { get; set; }
        public string lastStopLocation { get; set; }
        public string lastStopZipCode { get; set; }
        public int packs { get; set; }
        public int floorPallets { get; set; }
        public int totalPallets { get; set; }
        public decimal netWeight { get; set; }
        public decimal grossWeight { get; set; }
        public decimal cube { get; set; }
        public decimal meters { get; set; }
        public decimal cashValue { get; set; }
        public string cashCurrency { get; set; }
        public string cashPayment { get; set; }
        public string cashNote { get; set; }
        public int attachCount { get; set; }

        public XCMShipment XCMShipment { get; set; }
        public DateTime LastTrackingDate { get; set; }
        public override string ToString()
        {
            return $"{customerID}-{docDate}";
        }
    }

    public class XCMShipment
    {
        public string CustomerId { get; set; }
        public string DocNumber { get; set; }
        public string ExternRef { get; set; }

    }

}
