
using System;

public class RootobjectXCMRegistrations
{
    public ResultXCMRegistrations result { get; set; }
    public XCMRegistration[] registrations { get; set; }
}

public class ResultXCMRegistrations
{
    public object[] messages { get; set; }
    public string info { get; set; }
    public int maxPages { get; set; }
    public bool status { get; set; }
}

public class XCMRegistration
{
    public int id { get; set; }
    public int groupId { get; set; }
    public string siteID { get; set; }
    public string regTypeID { get; set; }
    public string regTypeGroup { get; set; }
    public string dateReg { get; set; }
    public string reference { get; set; }
    public string partNumber { get; set; }
    public string partNumberDescription { get; set; }
    public double totalQty { get; set; }
    public string batchNo { get; set; }
    public DateTime? dateExpire { get; set; }
    public string dateProd { get; set; }
    public double totalNetWeight { get; set; }
    public double totalGrossWeight { get; set; }
    public double totalCube { get; set; }
    public string customerID { get; set; }
    public string customerName { get; set; }
    public string ownerID { get; set; }
    public string ownerName { get; set; }
    public string anaID { get; set; }
    public string anaName { get; set; }
    public string note { get; set; }
}
