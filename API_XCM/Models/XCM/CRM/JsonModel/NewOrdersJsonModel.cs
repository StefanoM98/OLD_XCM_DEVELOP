using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_XCM.Models.XCM.CRM.JsonModel
{
    public class RootNewOrder
    {
        public HeaderNewOrder header { get; set; }
        public RowNewOrder[] rows { get; set; }
    }

    public class HeaderNewOrder
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
        public SenderNewOrder sender { get; set; }
        public ConsigneeNewOrder consignee { get; set; }
        public UnloadNewOrder unload { get; set; }
    }

    public class SenderNewOrder
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

    public class ConsigneeNewOrder
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

    public class UnloadNewOrder
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

    public class RowNewOrder
    {
        public string externalID { get; set; }
        public string partNumber { get; set; }
        public string packageID { get; set; }
        public string logWareId { get; set; }
        public int procID { get; set; }
        public string batchNo { get; set; }
        public string expireDate { get; set; }
        public string um { get; set; }
        public int qty { get; set; }
        public int unitNetWeight { get; set; }
        public int unitGrossWeight { get; set; }
        public int unitCube { get; set; }
        public int unitCostPrice { get; set; }
        public decimal unitSellPrice { get; set; }
        public int discount { get; set; }
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

}