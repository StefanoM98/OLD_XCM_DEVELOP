using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_XCM.Models.XCM
{
    public class ApsDafneResocontoModel
    {
        public int DocumentID { get; set; }
        public int RowID { get; set; }
        public string RiferimentoCliente { get; set; }
        public string RiferimentoDafne { get; set; }
        public string PartNumber { get; set; }
        public string PartNumberDes { get; set; }
        public int Quantita { get; set; }

    }
}