

using System;

public class RootobjectStock
{
    public ResultStock result { get; set; }
    public Stock[] stock { get; set; }
}

public class ResultStock
{
    public object[] messages { get; set; }
    public string info { get; set; }
    public int maxPages { get; set; }
    public bool status { get; set; }
}

public class Stock
{
    public string siteID { get; set; }
    public string customerID { get; set; }
    public string customerDes { get; set; }
    public string ownerID { get; set; }
    public string ownerDes { get; set; }
    public string logWareID { get; set; }
    public string partNumber { get; set; }
    public string partNumberDes { get; set; }
    public string um { get; set; }
    public string batchno { get; set; }
    public DateTime? dateExpire { get; set; }
    public string project { get; set; }
    public string subProject { get; set; }
    public decimal totalQty { get; set; }
    public decimal usableQty { get; set; }
    public decimal inUseQty { get; set; }
    public decimal availableQty { get; set; }
}

public class RootobjectOrdine
{
    public HeaderOrdine header { get; set; }
    public RowOrdine[] rows { get; set; }
}

public class HeaderOrdine
{
    public string docType { get; set; }
    public string siteID { get; set; }
    public string customerID { get; set; }
    public string ownerID { get; set; }
    public int anaType { get; set; }
    public string anaID { get; set; }
    public string reference { get; set; }
    public string referenceDate { get; set; }
    public string reference2 { get; set; }
    public string reference2Date { get; set; }
    public string externalID { get; set; }
    public string regTypeID { get; set; }
    public string logWareID { get; set; }
    public string dock { get; set; }
    public int procID { get; set; }
    public string shipdate { get; set; }
    public string internalNote { get; set; }
    public string publicNote { get; set; }
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
    public SenderOrdine sender { get; set; }
    public ConsigneeOrdine consignee { get; set; }
    public UnloadOrdine unload { get; set; }
}

public class SenderOrdine
{
    public int locationID { get; set; }
    public string description { get; set; }
    public string address { get; set; }
    public string zipCode { get; set; }
    public string location { get; set; }
    public string district { get; set; }
    public string region { get; set; }
    public string country { get; set; }
}

public class ConsigneeOrdine
{
    public int locationID { get; set; }
    public string description { get; set; }
    public string address { get; set; }
    public string zipCode { get; set; }
    public string location { get; set; }
    public string district { get; set; }
    public string region { get; set; }
    public string country { get; set; }
}

public class UnloadOrdine
{
    public int locationID { get; set; }
    public string description { get; set; }
    public string address { get; set; }
    public string zipCode { get; set; }
    public string location { get; set; }
    public string district { get; set; }
    public string region { get; set; }
    public string country { get; set; }
}

public class RowOrdine
{
    public int externalID { get; set; }
    public string partNumber { get; set; }
    public string packageID { get; set; }
    public string logWareId { get; set; }
    public int procID { get; set; }
    public string batchNo { get; set; }
    public string expireDate { get; set; }
    public string um { get; set; }
    public decimal qty { get; set; }
    public int unitNetWeight { get; set; }
    public int unitGrossWeight { get; set; }
    public int unitCube { get; set; }
    public int unitCostPrice { get; set; }
    public decimal unitSellPrice { get; set; }
    public decimal discount { get; set; }
    public string project { get; set; }
    public string subProject { get; set; }
    public string dock { get; set; }
    public string note { get; set; }
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


public class RootobjectResponeNewOrder
{
    public ResultResponeNewOrder result { get; set; }
    public int id { get; set; }
}

public class ResultResponeNewOrder
{
    public object[] messages { get; set; }
    public string info { get; set; }
    public int maxPages { get; set; }
    public bool status { get; set; }
}


