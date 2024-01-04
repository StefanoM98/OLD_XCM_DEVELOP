
public class RootobjectBEM
{
    public HeaderBEM header { get; set; }
    public RowBEM[] rows { get; set; }
}

public class HeaderBEM
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
    public SenderBEM sender { get; set; }
    public ConsigneeBEM consignee { get; set; }
    public UnloadBEM unload { get; set; }
}

public class SenderBEM
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

public class ConsigneeBEM
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

public class UnloadBEM
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

public class RowBEM
{
    public string externalID { get; set; }
    public string partNumber { get; set; }
    public string packageID { get; set; }
    public string logWareId { get; set; }
    public int procID { get; set; }
    public string batchNo { get; set; }
    public string expireDate { get; set; }
    public string um { get; set; }
    public decimal? qty { get; set; }
    public decimal? unitNetWeight { get; set; }
    public decimal? unitGrossWeight { get; set; }
    public decimal? unitCube { get; set; }
    public decimal? unitCostPrice { get; set; }
    public decimal? unitSellPrice { get; set; }
    public decimal? discount { get; set; }
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
