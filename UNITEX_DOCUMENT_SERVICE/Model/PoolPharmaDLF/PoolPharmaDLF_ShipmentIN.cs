using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNITEX_DOCUMENT_SERVICE.Model.PoolPharmaDLF
{
    public class PoolPharmaDLF_ShipmentIN
    {
        
        public string NumeroDocumento { get; set; }
        public int[] idxNumeroDocumento = new int[] { 0, 10 };
        public string RiferimentoEsterno { get; set; }
        public int[] idxRiferimentoEsterno = new int[] { 10, 15 };
        public string RagioneSocialeDestinatario { get; set; }
        public int[] idxRagioneSocialeDestinatario = new int[] { 25, 35 };
        public string IndirizzoDestinatario { get; set; }
        public int[] idxIndirizzoDestinatario = new int[] { 60, 35 };
        public string CittaDestinatario { get; set; }
        public int[] idxCittaDestinatario = new int[] { 95, 25 };
        public string CAPDestinatario { get; set; }
        public int[] idxCAPDestinatario = new int[] { 120, 9 };
        public string ProvDestinatario { get; set; }
        public int[] idxProvDestinatario = new int[] { 129, 2 };
        public string NazioneDestinatario { get; set; }
        public int[] idxNazioneDestinatario = new int[] { 134,35 };
        public string RagioneSocialeDestinazione { get; set; }
        public int[] idxRagioneSocialeDestinazione = new int[] { 134, 35 };//stesso valore di quello su, c'è un errore.....................
        public string IndirizzoDestinazione { get; set; }
        public int[] idxIndirizzoDestinazione = new int[] { 169, 35 };
        public string CittaDestinazione { get; set; }
        public int[] idxCittaDestinazione = new int[] { 204, 25 };
        public string CAPDestinazione { get; set; }
        public int[] idxCAPDestinazione = new int[] { 229, 9 };
        public string ProvDestinazione { get; set; }
        public int[] idxProvDestinazione = new int[] { 238, 2 };
        public string NazioneDestinazione { get; set; }
        public int[] idxNazioneDestinazione = new int[] { 240, 3 };
        public string Peso { get; set; }
        public int[] idxPeso = new int[] { 243, 7 };
        public string NumeroColli { get; set; }
        public int[] idxNumeroColli = new int[] { 250, 5 };
        public string ImportoContrassegno { get; set; }
        public int[] idxImportoContrassegno = new int[] { 255, 8 };
        public string DataBolla { get; set; }
        public int[] idxDataBolla = new int[] { 263, 8 };
        public string Note { get; set; }
        public int[] idxNote = new int[] { 271, 50 };
        public string Note1 { get; set; }
        public int[] idxNote1 = new int[] { 321, 50 };
        public string Note2 { get; set; }
        public int[] idxNote2 = new int[] { 371, 50 };
        public string MerceFragile { get; set; }
        public int[] idxMerceFragile = new int[] { 421, 1 };
        public string TemperaturaControllata { get; set; }
        public int[] idxTemperaturaControllata = new int[] { 422, 1 };
        public string TemperaturaMinoreDi25 { get; set; }
        public int[] idxTemperaturaMinoreDi25 = new int[] { 423, 1 };

    }
}
