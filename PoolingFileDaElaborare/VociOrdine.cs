using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolingFileDaElaborare
{
    public class VociOrdine
    {
        public DateTime DataOrdine { get; set; }
        public string NomeDestinazione { get; set; }
        public string IndirizzoDestinazione { get; set; }
        public string CAPDestinazione { get; set; }
        public string CittaDestinazione { get; set; }
        public string ProvDestinazione { get; set; }
        public string RagioneSocialeFatturazione { get; set; }
        public string PIVAFatturazione { get; set; }
        public string IndirizzoFatturazione { get; set; }
        public string CAPFatturazione { get; set; }
        public string CittaFatturazione { get; set; }
        public string ProvFatturazione { get; set; }
        public string NumeroOrdine { get; set; }
        public string CodProdotto { get; set; }
        public string DescrizioneProdotto { get; set; }
        public decimal QuantitaProdotto { get; set; }
        public decimal Sconto { get; set; }
        public decimal ImportoUnitario { get; set; }
        public decimal ImportoTotale { get; set; }
        public DateTime? DataConsegnaRichiesta { get; set; }
        public string Note { get; set; }
        public string Lotto { get; set; }
        public decimal IVA { get; set; }
        public string Barcode { get; set; }

        public string Regione { get; set; }
        public override string ToString()
        {
            return string.Join(" | ", NumeroOrdine, CodProdotto, DescrizioneProdotto,QuantitaProdotto, Lotto);
        }
    }
}
