using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCM_DOCUMENT_SERVICE.CodeReport
{
    public class GiacenzaProdotto
    {
        public DateTime DataRiferimento { get; set; }
        public string Riferimento { get; set; }
        public string CodiceProdotto { get; set; }
        public string DescrizioneProdotto { get; set; }
        public decimal QuantitaTotale { get; set; }
        public string MapID { get; set; }
        public string Lotto { get; set; }
        public DateTime DataScadenza { get; set; }
        public int ShelfLifeFromExpire { get; set; }
        public int ShelflifePrd { get; set; }
        public int GiorniRimanentiAllaVendita { get
            {
                return 0;
            } 
        }
        public string MagazzinoLogico { get; set; }

    }
}
