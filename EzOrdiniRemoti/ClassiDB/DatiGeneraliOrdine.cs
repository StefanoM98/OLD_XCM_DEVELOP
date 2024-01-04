using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzOrdiniRemoti
{
    public class DatiGeneraliOrdine
    {
        public DateTime DataOrdine { get; set; }
        public string NumeroOrdine { get; set; }
        public DateTime DataConsegnaRichiesta { get; set; }
        public string Note { get; set; }
    }
}
