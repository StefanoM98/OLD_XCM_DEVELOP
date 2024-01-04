using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_XCM.Models.XCM.CRM.JsonModel
{
    public class ShipmentJsonModel
    {
        public long ID_SHIPMENT { get; set; }
        public long Shipment_GespeID_UNITEX { get; set; }
        public string Shipment_OwnerAgency { get; set; }
        public string Shipment_DocNumber { get; set; }
        public Nullable<System.DateTime> Shipment_DocDate { get; set; }
        public Nullable<int> Shipment_StatusID { get; set; }
        public string Shipment_StatusType { get; set; }
        public string Shipment_StatusDes { get; set; }
        public Nullable<int> Shipment_WebStatusID { get; set; }
        public string Shipment_WebStatusType { get; set; }
        public Nullable<int> Shipment_WebOrderID { get; set; }
        public string Shipment_WebOrderNumber { get; set; }
        public string Shipment_InsideRef { get; set; }
        public string Shipment_ExternalRef { get; set; }
        public string Shipment_ServiceType { get; set; }
        public string Shipment_TransportType { get; set; }
        public string Shipment_CustomerID { get; set; }
        public string Shipment_CustomerDes { get; set; }
        public string Shipment_PickupSupplierID { get; set; }
        public string Shipment_PickupSupplierDes { get; set; }
        public Nullable<System.DateTime> Shipment_PickupDateTime { get; set; }
        public Nullable<System.DateTime> Shipment_DeliveryDateTime { get; set; }
        public string Shipment_SenderID { get; set; }
        public string Shipment_SenderDes { get; set; }
        public string Shipment_ConsineeID { get; set; }
        public string Shipment_ConsineeDes { get; set; }
        public Nullable<int> Shipment_FirstStopID { get; set; }
        public string Shipment_FirstStopDes { get; set; }
        public Nullable<int> Shipment_LastStopID { get; set; }
        public string Shipment_LastStopDes { get; set; }
        public Nullable<decimal> Shipment_Packs { get; set; }
        public Nullable<decimal> Shipment_FloorPallets { get; set; }
        public Nullable<decimal> Shipment_TotalPallets { get; set; }
        public Nullable<decimal> Shipment_NetWeight { get; set; }
        public Nullable<decimal> Shipment_GrossWeight { get; set; }
        public Nullable<decimal> Shipment_Cube { get; set; }
        public Nullable<decimal> Shipment_Meters { get; set; }
        public Nullable<decimal> Shipment_CashValue { get; set; }
        public string Shipment_CashCurrency { get; set; }
        public string Shipment_CashPayment { get; set; }
        public string Shipment_CashNote { get; set; }
        public Nullable<int> Shipment_AttachCount { get; set; }
        public long Shipment_GespeID_XCM { get; set; }
        public string Shipment_CustomerID_XCM { get; set; }
    }
}