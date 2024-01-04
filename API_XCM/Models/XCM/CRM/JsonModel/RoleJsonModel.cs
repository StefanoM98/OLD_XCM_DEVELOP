using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API_XCM.Models.XCM.CRM.JsonModel
{
    public class RoleJsonModel
    {
        [Key]
        public long Role_ID { get; set; }
        public string Role_Name { get; set; }
        public Nullable<bool> Role_Active { get; set; }
        public Nullable<System.DateTime> Role_CreationDate { get; set; }
        public Nullable<System.DateTime> Role_LastModifiedDate { get; set; }
    }
}