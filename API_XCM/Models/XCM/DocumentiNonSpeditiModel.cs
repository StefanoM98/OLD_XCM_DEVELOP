using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_XCM.Models.XCM
{
    public class DocumentiNonSpeditiModel
    {
        public string Mandante { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string RiferimentoExt { get; set; }
        public int Coverage { get; set; }
        public int Executed { get; set; }
        public Decimal Quantity { get; set; }
        public string StatoDocumento { get; set; }
        public int Evasi { get; set; }

        public string GespeID { get; set; }

    }

    //public class OrdiniEvasi
    //{

    //    public static int GetOrdiniEvasi(string mandante)
    //    {
    //        API_XCM.Code.XCM xcm = new API_XCM.Code.XCM();

    //        var res = xcm.GetResocontoDocumenti().FirstOrDefault(x => x.Mandante == mandante);
    //        if (res != null) return res.DeliveryOUT;
    //        else return 0;
            
    //    }
    //}
}



