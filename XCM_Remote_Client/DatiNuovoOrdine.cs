using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCM_Remote_Client
{
    public class TestataNuovoOrdine
    {
        public string RiferimentoOrdine { get; set; }
        public DateTime DataRiferimentoOrdine { get; set; }
        public string RagioneSociale { get; set; }
        public string Indirizzo { get; set; }
        public string CAP { get; set; }
        public string Provincia { get; set; }
        public string Regione { get; set; }
        public string Note { get; set; }
        public List<DatiNuovoOrdine> RigheOrdine { get; set; }
}
    public class DatiNuovoOrdine
    {
        public string CodiceProdotto { get; set; }
        public string LottoProdotto { get; set; }
        public string DescrizioneProdotto { get; set; }
        public decimal Quantita { get; set; }
        public int IVA { get; set; }
        public decimal Sconto { get; set; }
        public decimal ImportoUnitario { get; set; }
        //public string MagazzinoLogico { get; set; }
        public string UnitaDiMisuta { get; set; }
    }
}
