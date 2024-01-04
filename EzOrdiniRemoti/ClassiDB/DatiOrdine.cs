using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzOrdiniRemoti
{
    public class DatiOrdine
    {
        public DatiDestinatari _DatiDestinazione {get;set;}
        public DatiFatturazione _DatiFatturazione { get; set; }
        public DatiGeneraliOrdine _DatiGeneraliOrdine { get;set; }
        
        public string CodProdotto { get; set; }
        public string DescrizioneProdotto { get; set; }
        public decimal QuantitaProdotto { get; set; }
        public decimal Sconto { get; set; }
        public decimal ImportoUnitario { get; set; }
        public decimal ImportoTotale { get; set; }
        public string Lotto { get; set; }

        public decimal IVA { get; set; }
        public string Barcode { get; set; }

        public override string ToString()
        {
            return string.Join(" | ", CodProdotto, DescrizioneProdotto, Lotto, QuantitaProdotto, IVA);
        }
    }
}
