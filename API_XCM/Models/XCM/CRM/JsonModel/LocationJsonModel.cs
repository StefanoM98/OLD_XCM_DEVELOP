using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API_XCM.Models.XCM.CRM.JsonModel
{
    public class LocationJsonModel
    {
        [Key]
        public long Location_Id { get; set; }
        public string Location_Name { get; set; }
        public string Location_ZipCode { get; set; }
        public string Location_Location { get; set; }
        public string Location_Address { get; set; }
        public string Location_District { get; set; }
        public string Location_Region { get; set; }
        public string Location_Country { get; set; }
        public Nullable<bool> Location_IsDefault { get; set; }
        public System.DateTime Location_LastModifiedDate { get; set; }
        public System.DateTime Location_CreationDate { get; set; }
        public string Location_LastModifiedUserId { get; set; }
        public Nullable<int> Location_GespeLocationId { get; set; }
        public Nullable<bool> Location_IsActive { get; set; }
        public string FK_Location_Customer_ID { get; set; }

    }
}