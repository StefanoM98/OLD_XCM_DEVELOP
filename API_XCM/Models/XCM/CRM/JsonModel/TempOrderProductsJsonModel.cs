using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_XCM.Models.XCM.CRM.JsonModel
{
    public class TempOrderProductsJsonModel
    {
        public long ID { get; set; }
        public long Order_ID { get; set; }
        public string Product_PartNumber { get; set; }
        public string Product_Des { get; set; }
        public int Product_Quantity { get; set; }
        public decimal Product_Price { get; set; }
        public decimal Product_Discount { get; set; }
        public decimal Product_Iva { get; set; }
    }
}