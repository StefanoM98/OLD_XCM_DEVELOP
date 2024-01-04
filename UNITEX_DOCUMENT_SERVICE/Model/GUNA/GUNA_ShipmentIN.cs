using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNITEX_DOCUMENT_SERVICE.Model.GUNA
{
    public class GUNA_ShipmentIN
    {
        ///annoSpedizione
        public string AASPED { get; set; }
        public int[] idxAASPED = new int[] { 0, 4 };

        ///mese giorno spedizione
        public string MMGGSPED { get; set; }
        public int[] idxMMGGSPED = new int[] { 5, 4 };

        ///numero bolla di spedizione (in caso di fatturazione differita) o fattura contabile (in caso di fatturazione immediata con fattura accompagnatoria)- univoco nell'anno
        public string NRSPED { get; set; }
        public int[] idxNRSPED = new int[] { 10, 7 };

        ///ragione sociale destinatario
        public string RAGSOC1 { get; set; }
        public int[] idxRAGSOC1 = new int[] { 18, 35 };

        ///estensione ragione sociale destinatario
        public string RAGSOC2 { get; set; }
        public int[] idxRAGSOC2 = new int[] { 53, 35 };

        ///indirizzo destinatario
        public string ADDRESS { get; set; }
        public int[] idxADDRESS = new int[] { 88, 35 };

        ///cap destinatario
        public string PTCODE { get; set; }
        public int[] idxPTCODE = new int[] { 123, 9 };

        ///località destinatario
        public string CITY { get; set; }
        public int[] idxCITY = new int[] { 132, 35 };

        ///provincia destinatario
        public string REGION { get; set; }
        public int[] idxREGION = new int[] { 167, 2 };

        ///nazione destinatario
        public string COUNTRY { get; set; }
        public int[] idxCOUNTRY = new int[] { 170, 3 };

        ///numero totale colli spedizione
        public string NCOLLI { get; set; }
        public int[] idxNCOLLI = new int[] { 174, 5 };

        ///totale peso kg bollettato - peso lordo complessivo spedizione
        public string PESO { get; set; }
        public int[] idxPESO = new int[] { 180, 9 };

        ///totale volume m3 bollettato - volume spedizione espresso in metri cubi
        public string VOLUME { get; set; }
        public int[] idxVOLUME = new int[] { 190, 7 };

        ///importo in contrassegno di cui la merce è gravata
        public string C_ASS { get; set; }
        public int[] idxC_ASS = new int[] { 198, 15 };

        ///tipo incasso contrassegno
        public string TP_INCASSO { get; set; }
        public int[] idxTP_INCASSO = new int[] { 214, 5 };

        ///divisa importo contrassegno (EUR)
        public string DIVISA_C_ASS { get; set; }
        public int[] idxDIVISA_C_ASS = new int[] { 220, 3 };

        ///Numero Ordine di Vendita
        public string RIFMITT_C { get; set; }
        public int[] idxRIFMITT_C = new int[] { 224, 15 };

        ///Telefono1 Sarà riportato il numero di cellulare dell'anagrafica dello ship-to se presente.
        ///Se non è presente:
        ///- se lo ship-to ha il numero fisso allora il campo rimarrà blank 
        ///- se lo ship-to non ha nemmeno il numero fisso allora sarà riportato il numero di cellulare dell'anagrafica del sold-to se presente
        public string TEL1 { get; set; }
        public int[] idxTEL1 = new int[] { 240, 20 };

        ///Telefono2 Sarà riportato il numero di cellulare dell'anagrafica dello ship-to se presente.
        ///Se non è presente:
        ///- se lo ship-to ha il numero fisso allora il campo rimarrà blank 
        ///- se lo ship-to non ha nemmeno il numero fisso allora sarà riportato il numero di cellulare dell'anagrafica del sold-to se presente
        public string TEL2 { get; set; }
        public int[] idxTEL2 = new int[] { 261, 20 };

        ///Telefono3 Sarà riportato il numero di cellulare dell'anagrafica dello ship-to se presente.
        ///Se non è presente:
        ///- se lo ship-to ha il numero fisso allora il campo rimarrà blank 
        ///- se lo ship-to non ha nemmeno il numero fisso allora sarà riportato il numero di cellulare dell'anagrafica del sold-to se presente
        public string TEL3 { get; set; }
        public int[] idxTEL3 = new int[] { 282, 20 };

        ///descrizione note - Eventuali annotazioni del Mittente per il corriere
        public string NOTE1 { get; set; }
        public int[] idxNOTE1 = new int[] { 303, 35 };

        ///descrizione note - segue campo precedente
        public string NOTE2 { get; set; }
        public int[] idxNOTE2 = new int[] { 338, 35 };

        ///Appuntamento ("A" o blank)
        public string CONS_PART1 { get; set; }
        public int[] idxCONS_PART1 = new int[] { 373, 1 };

        ///Appuntamento ("A" o blank)
        public string EOL { get; set; }
        public int[] idxEOL = new int[] { 374, 1 };

        ///Tipo record + Ultimi 10 caratteri HU SAP (VEKP-EXIDV) NB. ho deciso di trattarli come unico campo PD
        public string segnacollo { get; set; }
        public int[] idxSegnacollo = new int[] { 0, 9 };


        ///segnacolli spedizioni GUNA
        public List<string> Segnacolli = new List<string>();

    }
}
