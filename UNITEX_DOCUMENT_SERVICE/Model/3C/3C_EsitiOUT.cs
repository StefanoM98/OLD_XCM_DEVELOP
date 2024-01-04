using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNITEX_DOCUMENT_SERVICE.Model._3C
{
    public class _3C_EsitiOUT
    {

        ///per il dettaglio barcode letti in ingresso da parte del corrispondente prevediamo:						
        ///a posizione 163 - tipo evento indicare  I = ingresso (il collo indicato è stato accettato in ingresso)						
        ///U = uscita (il collo indicato è stato caricato per la distribuzione)						
        ///a posizione 232 - 261 indicare il barcode id collo 		
        ///
        ///-------------------------------------------------------------------------------------------------------------------
        /// 
        /// <summary>
        ///Il tipo evento dovrà corrispondere alla tabella seguente:
        ///
        ///Q = Presa in carico - (arrivata al magazzino di distribuzione)
        ///B = In transito verso xx   (xx rappresenta il deposito di destino, da indicare nel campo note)
        ///                                        indica lo spostamento merce da vs. deposito ad altra filiale / corrispondente
        ///C = arrivata presso il deposito di xx  (xx rappresenta il deposito di destino, da indicare nel campo note)
        ///P = Consegna Provvisoria (in consegna)
        ///D = Consegnata Definitivamente
        ///M = Mancata Consegna (dovrà essere compilato il campo Descizione Evento con la motivazione)
        ///T = Consegna Prenotata (nel caso in cui venga fissato un appuntamento per la consegna.
        ///                                     Da trasmettere prima della consegna stessa)
        ///G = Giacenza (nel campo Descrizione Evento dovrà essere indicata la motivazione di apertura giacenza)
        ///K = Subaffidata a corrispondente
        ///
        ///Per tutti gli eventi di tipo P dovrà seguire un esito di tipo D,M,G o K
        ///Per tutti gli eventi di tipo K dovrà seguire un esito di tipo D,M o G
        ///Per tutti gli eventi di tipo M dovrà seguire un nuovo esito di qualsiasi tipo.
        ///Per tutti gli eventi di tipo T dovrà seguire un nuovo esito di qualsiasi tipo.
        /// </summary>
        public string NumeroBolla { get; set; }
        public int[] idxNumeroBolla = new int[] { 0, 15 };

        public string Filler { get; set; }
        public int[] idxFiller = new int[] { 15, 9 };

        public string CodiceMittente { get; set; }
        public int[] idxCodiceMittente = new int[] { 24, 6 };

        public string RagioneSocialeMittente { get; set; }
        public int[] idxRagioneSocialeMittente = new int[] { 30, 40 };

        public string RagioneSocialeDestinatario { get; set; }
        public int[] idxRagioneSocialeDestinatario = new int[] { 70, 40 };

        public string LocalitaDestinatario { get; set; }
        public int[] idxLocalitaDestinatario = new int[] { 110, 30 };

        public string ProvDestinatario { get; set; }
        public int[] idxProvDestinatario = new int[] { 140, 2 };

        public string NumeroColli { get; set; }
        public int[] idxNumeroColli = new int[] { 142, 5 };

        public string Peso1D { get; set; }//1dec
        public int[] idxPeso1D = new int[] { 147, 7 };

        public string DataEvento { get; set; }//
        public int[] idxDataEvento = new int[] { 154, 8 };

        public string TipoEvento { get; set; }
        public int[] idxTipoEvento = new int[] { 162, 1 };

        public string DataPrenotazione { get; set; }
        public int[] idxDataPrenotazione = new int[] { 163, 1 };

        public string DescrizioneEvento { get; set; }
        public int[] idxDescrizioneEvento = new int[] { 164, 60 };

        public string Progressivo { get; set; }
        public int[] idxProgressivo = new int[] { 224, 3 };

        public string OrarioEvento { get; set; }
        public int[] idxOrarioEvento = new int[] { 227, 4 };

        public string BarcodeSegnacolloLetto { get; set; }
        public int[] idxBarcodeSegnacolloLetto = new int[] { 231, 30 };

        public string Filler2 { get; set; }
        public int[] idxFiller2 = new int[] { 261, 38 };

        public string FineRecord { get { return "."; } }

        public int statoUNITEX { get; set; }


    }
}
