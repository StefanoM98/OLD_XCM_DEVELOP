
using System;

public class RootobjectDocument
{
    public ResultDocument result { get; set; }
    public Document[] documents { get; set; }
}

public class ResultDocument
{
    public object[] messages { get; set; }
    public string info { get; set; }
    public int maxPages { get; set; }
    public bool status { get; set; }
}

public class Document
{
    public int id { get; set; }
    public string docNumber { get; set; }
    public string docType { get; set; }
    public DateTime docDate { get; set; }
    public string siteID { get; set; }
    public int shipID { get; set; }
    public string shipDocNumber { get; set; }
    public int tripID { get; set; }
    public string tripDocNumber { get; set; }
    public int statusId { get; set; }
    public string statusDes { get; set; }
    public string reference { get; set; }
    public DateTime referenceDate { get; set; }
    public string reference2 { get; set; }
    public DateTime reference2Date { get; set; }
    public string externalID { get; set; }
    public string regTypeID { get; set; }
    public string customerID { get; set; }
    public string customerDes { get; set; }
    public string ownerID { get; set; }
    public string ownerDes { get; set; }
    public int senderID { get; set; }
    public string senderDes { get; set; }
    public string senderAddress { get; set; }
    public string senderZipCode { get; set; }
    public string senderLocation { get; set; }
    public string senderDistrict { get; set; }
    public string senderRegion { get; set; }
    public string senderCountry { get; set; }
    public int consigneeID { get; set; }
    public string consigneeDes { get; set; }
    public string consigneeAddress { get; set; }
    public string consigneeZipCode { get; set; }
    public string consigneeLocation { get; set; }
    public string consigneeDistrict { get; set; }
    public string consigneeRegion { get; set; }
    public string consigneeCountry { get; set; }
    public int unloadID { get; set; }
    public string unLoadDes { get; set; }
    public string unloadAddress { get; set; }
    public string unloadZipCode { get; set; }
    public string unloadLocation { get; set; }
    public string unloadDistrict { get; set; }
    public string unloadRegion { get; set; }
    public string unloadCountry { get; set; }
    public int rowsNo { get; set; }
    public double totalQty { get; set; }
    public double totalPacks { get; set; }
    public double totalBoxes { get; set; }
    public float totalNetWeight { get; set; }
    public float totalGrossWeight { get; set; }
    public double totalCube { get; set; }
    public double coverage { get; set; }
    public double planned { get; set; }
    public double executed { get; set; }
    public string internalNote { get; set; }
    public object publicNode { get; set; }
    public string deliveryNote { get; set; }
    public string info1 { get; set; }
    public string info2 { get; set; }
    public string info3 { get; set; }
    public string info4 { get; set; }
    public string info5 { get; set; }
    public string info6 { get; set; }
    public string info7 { get; set; }
    public string info8 { get; set; }
    public string info9 { get; set; }
}


