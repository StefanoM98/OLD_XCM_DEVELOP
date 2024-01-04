using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API_XCM.Models.XCM.API
{
    public class RoleViewModel
    {

        [Key]
        public long Role_ID { get; set; }
        public string Role_Name { get; set; }
        public bool? Role_Active { get; set; }
        public DateTime? Role_CreationDate { get; set; }
        public DateTime? Role_LastModifiedDate { get; set; }

    }
}