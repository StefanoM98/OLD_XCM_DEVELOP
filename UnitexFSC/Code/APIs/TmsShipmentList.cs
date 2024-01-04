using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace UnitexFSC.Code.APIs
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
        public bool isLocked { get; set; }
        public int statusId { get; set; }
        public string statusType { get; set; }
        public string statusDes { get; set; }
        public string statusColor { get; set; }
        public int webStatusId { get; set; }
        public string webStatusType { get; set; }
        public string webStatusDes { get; set; }
        public string webStatusColor { get; set; }
        public int webOrderID { get; set; }
        public string webOrderNumber { get; set; }
        public string insideRef { get; set; }
        public string externRef { get; set; }
        public string serviceType { get; set; }
        public string transportType { get; set; }
        public string customerID { get; set; }
        public string customerDes { get; set; }
        public string customerAddress { get; set; }
        public string customerLocation { get; set; }
        public string customerZipCode { get; set; }
        public string customerDistrict { get; set; }
        public string customerCountry { get; set; }
        public string pickupSupplierID { get; set; }
        public string pickupSupplierDes { get; set; }
        public string deliverySupplierID { get; set; }
        public string deliverySupplierDes { get; set; }
        public object pickupDateTime { get; set; }
        public object deliveryDateTime { get; set; }
        public string senderID { get; set; }
        public string senderDes { get; set; }
        public string senderAddress { get; set; }
        public string senderLocation { get; set; }
        public string senderZipCode { get; set; }
        public string senderDistrict { get; set; }
        public string senderCountry { get; set; }
        public string consigneeID { get; set; }
        public string consigneeDes { get; set; }
        public string consigneeAddress { get; set; }
        public string consigneeLocation { get; set; }
        public string consigneeZipCode { get; set; }
        public string consigneeDistrict { get; set; }
        public string consigneeCountry { get; set; }
        public int firstStopID { get; set; }
        public string firstStopDes { get; set; }
        public string firstStopAddress { get; set; }
        public string firstStopLocation { get; set; }
        public string firstStopZipCode { get; set; }
        public string firstStopDistrict { get; set; }
        public string firstStopCountry { get; set; }
        public int lastStopID { get; set; }
        public string lastStopDes { get; set; }
        public string lastStopAddress { get; set; }
        public string lastStopLocation { get; set; }
        public string lastStopZipCode { get; set; }
        public string lastStopDistrict { get; set; }
        public string lastStopCountry { get; set; }
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
        public int recUserID { get; set; }
        public DateTime recTimeStamp { get; set; }
    }

}