using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_XCM.Models.XCM.CRM
{
    public class CommonCustomerList
    {
        public CustomerResult result { get; set; }
        public CustomerEspritecAPI[] customers { get; set; }
    }

    public class CustomerResult
    {
        public object[] messages { get; set; }
        public string info { get; set; }
        public int maxPages { get; set; }
        public bool status { get; set; }
    }

    public class CustomerEspritecAPI
    {
        public string id { get; set; }
        public string description { get; set; }
        public bool isEnable { get; set; }
        public string address { get; set; }
        public string zipCode { get; set; }
        public string location { get; set; }
        public string district { get; set; }
        public string country { get; set; }
        public string defaultPriceListId { get; set; }
        public string vatCode { get; set; }
        public string fiscalCode { get; set; }
    }
}