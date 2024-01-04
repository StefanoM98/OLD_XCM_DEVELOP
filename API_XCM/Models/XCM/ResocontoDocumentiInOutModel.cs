using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_XCM.Models
{
    public class ResocontoDocumentiInOutModel
    {
        public string Mandante { get; set; }
        public int DeliveryOUT { get; set; }
        public int DeliveryIN{ get; set; }
        public int QuantitaTotaleDeliveryOUT { get; set; }
        public int QuantitaTotaleDeliveryIN { get; set; }
    }
}