using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API_XCM.Models.XCM.CRM.JsonModel
{
    public class CustomerAuthorizationJsonModel
    {
        [Key]
        public string Customer_Id { get; set; }
        public bool Warehouse_Active { get; set; }
        public bool Orders_Active { get; set; }
        public bool Tracking_Active { get; set; }

        public bool Agents_Active { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public string LastModifiedUserID { get; set; }
        public DateTime? ExpireDateTracking { get; set; }
        public DateTime? ExpireDateWarehouse { get; set; }
        public DateTime? ExpireDateOrders { get; set; }
        public DateTime? ExpireDateAgents { get; set; }
    }
}