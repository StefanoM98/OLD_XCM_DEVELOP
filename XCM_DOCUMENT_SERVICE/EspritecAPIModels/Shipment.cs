
using System;

public class RootobjectShipment
{
    public ResultShipment result { get; set; }
    public Shipment shipment { get; set; }
}   

public class ResultShipment
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
    public DateTime? docDate { get; set; }
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
    public string pickupSupplierID { get; set; }
    public string pickupSupplierDes { get; set; }
    public string deliverySupplierID { get; set; }
    public string deliverySupplierDes { get; set; }
    public DateTime? pickupDateTime { get; set; }
    public DateTime? deliveryDateTime { get; set; }
    public string senderID { get; set; }
    public string senderDes { get; set; }
    public string consigneeID { get; set; }
    public string consigneeDes { get; set; }
    public decimal packs { get; set; }
    public decimal floorPallets { get; set; }
    public decimal totalPallets { get; set; }
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
}
