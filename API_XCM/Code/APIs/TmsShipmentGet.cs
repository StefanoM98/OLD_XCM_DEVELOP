using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace API_XCM.Code.APIs
{

    public class TmsShipmentGet
    {
        public TmsShipmentGetResult result { get; set; }
        public TmsShipmentGetShipment shipment { get; set; }
    }

    public class TmsShipmentGetResult
    {
        public object[] messages { get; set; }
        public string info { get; set; }
        public int maxPages { get; set; }
        public bool status { get; set; }
    }

    public class TmsShipmentGetShipment
    {
        public int id { get; set; }
        public string ownerAgency { get; set; }
        public string docNumber { get; set; }
        public DateTime docDate { get; set; }
        public int statusId { get; set; }
        public string statusDes { get; set; }
        public string statusType { get; set; }
        public int webStatusId { get; set; }
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
        public string customerCountry { get; set; }
        public string pickupSupplierID { get; set; }
        public string pickupSupplierDes { get; set; }
        public string deliverySupplierID { get; set; }
        public string deliverySupplierDes { get; set; }
        public DateTime pickupDateTime { get; set; }
        public string deliveryDateTime { get; set; }
        public string senderID { get; set; }
        public string senderDes { get; set; }
        public string senderAddress { get; set; }
        public string senderLocation { get; set; }
        public string senderZipcode { get; set; }
        public string senderCountry { get; set; }
        public string consigneeID { get; set; }
        public string consigneeDes { get; set; }
        public string consigneeAddress { get; set; }
        public string consigneeLocation { get; set; }
        public string consigneeZipcode { get; set; }
        public string consigneeCountry { get; set; }
        public int packs { get; set; }
        public int floorPallets { get; set; }
        public int totalPallets { get; set; }
        public decimal netWeight { get; set; }
        public decimal grossWeight { get; set; }
        public decimal cube { get; set; }
        public decimal meters { get; set; }
        public string incoterm { get; set; }
        public decimal cashValue { get; set; }
        public string cashCurrency { get; set; }
        public string cashPayment { get; set; }
        public string cashNote { get; set; }
        public string cashStatus { get; set; }
        public string internalNote { get; set; }
        public string publicNote { get; set; }
    }

}