using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzOrdiniRemoti
{
    public class DatiFatturazione
    {
        public string RagioneSocialeFatturazione { get; set; }
        public string PivaFatturazione { get; set; }
        public string IndirizzoFatturazione { get; set; }
        public string CapFatturazione { get; set; }
        public string CittaFatturazione { get; set; }
        public string ProvFatturazione { get; set; }

        public override string ToString()
        {
            return string.Join(" | ", RagioneSocialeFatturazione, PivaFatturazione, IndirizzoFatturazione, CapFatturazione, CittaFatturazione);
        }
    }
}
