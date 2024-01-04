using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API_XCM.Models.XCM.CRM.JsonModel
{
    public class ClientJsonModel
    {
        [Key]
        public string UserClient_UserID { get; set; }
        public string UserClient_Name { get; set; }
        public DateTime UserClient_CreationDate { get; set; }
        public DateTime UserClient_LastModifiedDate { get; set; }
        public string UserClient_LastModifiedUserID { get; set; }

        public string FK_UserClient_Customer_ID { get; set; }

        public LocationJsonModel UserClient_Location { get; set; }
    }
}