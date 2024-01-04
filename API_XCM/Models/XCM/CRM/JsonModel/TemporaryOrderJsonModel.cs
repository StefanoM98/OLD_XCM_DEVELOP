using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_XCM.Models.XCM.CRM.JsonModel
{
    public class TemporaryOrderJsonModel
    {
        public TempOrderJsonModel Data { get; set; }
        public List<ProductJsonModel> Products { get; set; }
    }
}