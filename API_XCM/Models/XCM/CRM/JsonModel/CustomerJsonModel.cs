using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API_XCM.Models.XCM.CRM.JsonModel
{
    public class CustomerJsonModel
    {
        [Key]
        public string Customer_id { get; set; }
        public string Customer_SessionID { get; set; }
        public string Customer_description { get; set; }
        public bool Customer_isEnable { get; set; }
        public string Customer_address { get; set; }
        public string Customer_zipCode { get; set; }
        public string Customer_location { get; set; }
        public string Customer_district { get; set; }
        public string Customer_country { get; set; }
        public string Customer_defaultPriceListId { get; set; }
        public string Customer_vatCode { get; set; }
        public bool Customer_IsEnableCRM { get; set; }
        public byte[] Customer_Logo { get; set; }
        public System.DateTime Customer_CreationDate { get; set; }
        public System.DateTime Customer_LastModifiedDate { get; set; }
        public string Customer_LastModifiedUserID { get; set; }
        public CustomerAuthorizationJsonModel Customer_Authorization { get; set; }
    }
}