using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNITEX_DOCUMENT_SERVICE.Model.DAMORA
{
    public class DAMORA_ShipmentIN
    {
        public string Spedvet { get; set; }//Numero di spedizione
        public int[] idxSpedvet = new int[] { 0, 20 };
        public string Datavet { get; set; }//Data spedizione - GGMMAA
        public int[] idxDatavet = new int[] { 0, 6 };
        public string Portovet { get; set; }//Porto o tipo spedizione - F=Franco – A=Assegnato
        public int[] idxPortovet = new int[] { 0, 1 };
        public string Mittvet { get; set; }//Codice mittente
        public int[] idxMittvet = new int[] { 0, 8 };
        public string Descmitvet { get; set; }// Ragione sociale mittente 
        public int[] idxDescmitvet = new int[] { 0, 30 };
        public string Indmitvet { get; set; }//Indirizzo mittente
        public int[] idxIndmitvet = new int[] { 0, 30 };
        public string Capmitvet { get; set; }//Cap mittente
        public int[] idxCapmitvet = new int[] { 0, 5 };
        public string Locmitvet { get; set; }//Località mittente
        public int[] idxLocmitvet = new int[] { 0, 30 };
        public string Provmitvet { get; set; }//Provincia mittente
        public int[] idxProvmitvet = new int[] { 0, 2 };
        public string Destvet { get; set; }//Codice destinatario
        public int[] idxDestvet = new int[] { 0, 8 };
        public string Descdesvet { get; set; }//Ragione sociale destinatario
        public int[] idxDescdesvet = new int[] { 0, 30 };
        public string Inddesvet { get; set; }//Indirizzo destinatario
        public int[] idxInddesvet = new int[] { 0, 30 };
        public string Capdesvet { get; set; }//Cap destinatario
        public int[] idxCapdesvet = new int[] { 0, 5 };
        public string Locdesvet { get; set; }//Località destinatario
        public int[] idxLocdesvet = new int[] { 0, 30 };
        public string Provdesvet { get; set; }//Provincia destinatario
        public int[] idxProvdesvet = new int[] { 0, 2 };
        public string Bordvet { get; set; }//Numero del Bordereau
        public int[] idxBordvet = new int[] { 0, 6 };
        public string Datborvet { get; set; }//Data Bordereau - GGMMAA??
        public int[] idxDatborvet = new int[] { 0, 6 };
        public string Notevet { get; set; }//Note Annotazioni
        public int[] idxNotevet = new int[] { 0, 50 };
        public string Notevet1 { get; set; }//Note Istruzioni
        public int[] idxNotevet1 = new int[] { 0, 50 };
        public string Notelet { get; set; }//altre Note Istruzioni
        public int[] idxNotelet = new int[] { 0, 50 };
        public string DataTassativaConsegna { get; set; }//Data tassativa di Consegna
        public int[] idxDataTassativaConsegna = new int[] { 0, 6 };
        public string Tariffa { get; set; }//Tipo Tariffa
        public int[] idxTariffa = new int[] { 0, 8 };
        public string Collvet { get; set; }//Numero colli
        public int[] idxCollvet = new int[] { 0, 10 };
        public string Volumvet { get; set; }//Volume - 3dec
        public int[] idxVolumvet = new int[] { 0, 10 };
        public string Pesovet { get; set; }//Peso - 3dec
        public int[] idxPesovet = new int[] { 0, 10 };
        public string Bollivet { get; set; }//Riferimento bolla cliente 
        public int[] idxBollivet = new int[] { 0, 30 };
        public string Contrvet { get; set; }//Importo contrassegno - 2dec
        public int[] idxContrvet = new int[] { 0, 10 };
        public string Barcode { get; set; }//Barcode spedizione
        public int[] idxBarcode = new int[] { 0, 30 };

    }

    public class DAMORA_EsitiOUT
    {
        public string rifExt { get; set; }
        public string dataEsito { get; set; }
        public string DescrizioneEsito { get; set; }
    }
}
