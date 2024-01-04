
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
    public bool isLocked { get; set; }
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
    public DateTime? pickupDateTime { get; set; }
    public DateTime? deliveryDateTime { get; set; }
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
    public double floorPallets { get; set; }
    public double totalPallets { get; set; }
    public double netWeight { get; set; }
    public double grossWeight { get; set; }
    public double cube { get; set; }
    public double meters { get; set; }
    public string incoterm { get; set; }
    public double cashValue { get; set; }
    public string cashCurrency { get; set; }
    public string cashPayment { get; set; }
    public string cashNote { get; set; }
    public string cashStatus { get; set; }
    public string internalNote { get; set; }
    public string publicNote { get; set; }
}
