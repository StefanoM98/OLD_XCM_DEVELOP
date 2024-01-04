using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_XCM.Models.XCM.CRM.SyncroDB_CRM
{
    public class RootObjectTmsShipmentList
    {
        public TmsShipmentListResult result { get; set; }
        public ShipmentListJson[] shipments { get; set; }
    }
    public class TmsShipmentListResult
    {
        public object[] messages { get; set; }
        public string info { get; set; }
        public int maxPages { get; set; }
        public bool status { get; set; }
    }
    public class ShipmentListJson
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
        public DateTime deliveryDateTime { get; set; }
        public string senderID { get; set; }
        public string senderDes { get; set; }
        public string consigneeID { get; set; }
        public string consigneeDes { get; set; }
        public int firstStopID { get; set; }
        public string firstStopDes { get; set; }
        public int lastStopID { get; set; }
        public string lastStopDes { get; set; }
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
    }    
    public class ShipmentViewModel
    {
        public int Shipment_id { get; set; }
        public string Shipment_docNumber { get; set; }
        public DateTime Shipment_docDate { get; set; }
        public int Shipment_statusId { get; set; }
        public string Shipment_statusDes { get; set; }
        public string Shipment_insideRef { get; set; }
        public string Shipment_externRef { get; set; }
        public string Shipment_serviceType { get; set; }
        public string Shipment_transportType { get; set; }
        public string Shipment_customerID { get; set; }
        public string Shipment_customerDes { get; set; }
        public string Shipment_senderDes { get; set; }
        public string Shipment_consigneeDes { get; set; }
        public int Shipment_packs { get; set; }
        public int Shipment_floorPallets { get; set; }
        public int Shipment_totalPallets { get; set; }
        public decimal Shipment_netWeight { get; set; }
        public decimal Shipment_grossWeight { get; set; }
        public decimal Shipment_cube { get; set; }
        public decimal Shipment_meters { get; set; }

        public XCMShipment XCMShipment { get; set; }
    }
    public class XCMShipment
    {
        public string CustomerId { get; set; }
        public string DocNumber { get; set; }
        public string ExternRef { get; set; }

    }

}