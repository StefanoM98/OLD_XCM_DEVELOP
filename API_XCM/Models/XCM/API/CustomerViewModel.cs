using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API_XCM.Models.XCM.API
{
    public class CustomerViewModel
    {
        [Key]
        public string Customer_ID { get; set; }
        public string Customer_description { get; set; }
    }
}