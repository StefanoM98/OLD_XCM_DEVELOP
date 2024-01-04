
public class RootobjectUpdateDocument
{
    public HeaderUpdateDocument header { get; set; }
}

public class HeaderUpdateDocument
{
    public int id { get; set; }
    public int anaType { get; set; }
    public string anaID { get; set; }
    public string reference { get; set; }
    public string referenceDate { get; set; }
    public string reference2 { get; set; }
    public string reference2Date { get; set; }
    public string externalID { get; set; }
    public string dock { get; set; }
    public string logWareID { get; set; }
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
    public SenderUpdateDocument sender { get; set; }
    public ConsigneeUpdateDocument consignee { get; set; }
    public UnloadUpdateDocument unload { get; set; }
}

public class SenderUpdateDocument
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

public class ConsigneeUpdateDocument
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

public class UnloadUpdateDocument
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
