using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzOrdiniRemoti
{
    public class DatiDestinatari
    {
        public string NomeDestinazione { get; set; }
        public string IndirizzoDestinazione { get; set; }
        public string CAPDestinazione { get; set; }
        public string CittaDestinazione { get; set; }
        public string ProvDestinazione { get; set; }

        public override string ToString()
        {
            return string.Join(" | ", NomeDestinazione, IndirizzoDestinazione, CAPDestinazione, CittaDestinazione, ProvDestinazione);
        }
    }
}
