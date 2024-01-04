using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCM_DOCUMENT_SERVICE
{
    public class InterpreteInvioCDLBolla
    {
        public string BARCODE_BOLLA { get; set; }//1-14 | BARCODE LDV
        public string TIPO_SPEDIZIONE { get; set; }//16-18 | **** tabella di decodifica dei tipi boll
        public DateTime DATA_SPEDIZIONE { get; set; }//19-26
        public int ID_MITT { get; set; }//30-37 | ???
        public int ID_CONTRATTO { get; set; }//40-47 | ???
        public string MITTENTE { get; set; }//53-82 
        public string INDIRIZZO_MITTENTE { get; set; }//83-112
        public string LOCALITA_MITTENTE { get; set; }//113-142
        public string CAP_MITTENTE { get; set; }//153-162
        public string NUMERO_DDT_MITTENTE { get; set; }//163-178
        public int ESERCIZIO_ANNO_BOLLA { get; set; }//187-190
        public int FILIALE { get; set; }//191-192 | ??
        public int PROGRESSIVO_BOLLA { get; set; }//193-200
        public string ALTRO_RIFERIMENTO { get; set; }//201-220
        public string SEC_RIFERIMENTI { get; set; }//221-240
        public string RAG_SOC_DESTINATARIO { get; set; }//243-272
        public string INDIRIZZO_DESTINATARIO { get; set; }//273-302
        public string LOCALITA_DESTINATARIO { get; set; }//303-332
        public string PROVINCIA_DESTINATARIO { get; set; }//333-337
        public string CAP_DESTINATARIO { get; set; }//343--352
        public string MITTENTE_ORIGINALE { get; set; }//353-362
        public string PROVINCIA_MITTENTE_ORIGINALE { get; set; }//368-372 | ??
        public int COLLI { get; set; }//373-377
        public decimal PESO { get; set; }//378-385 | 3 decimali
        public int VOLUME { get; set; }//386-393 | 3 decimali
        public int BANCALI { get; set; }//394-396
        public int EPAL { get; set; }//397-399
        public decimal METRI_LINEARI { get; set; }//400-406 | 2 decimali
        public string TIPO_INCASSO_RICHIESTO { get; set; }//412-412 | A=Assegno Intestato | C=Contanti | D=Assegno | B=Bonifico
        public int ANTICIPATE { get; set; }//429-441 | 4 decimali | servizio accessorio anticipo acquisto
        public int PRIMO_SEGNACOLLO { get; set; }//443-450
        public string TIPO_SEGNACOLLO { get; set; }//451-452 | X o A (Vuol dire AL) ????????
        public int ULTIMO_SEGNACOLLO { get; set; }//453-460
        public string TIPO_ADR_PRINCIPALE { get; set; }//461-465 | Numero della classe di merce pericolosa (2,3,4.1,4.2,4.3,4.1,5.2,8,9)
        public string ESPRESSO { get; set; }//466-466 | S/N : Indica se è un servizio Espresso
        public string IN_USO_PER_ALTRE_LOGICHE { get; set; }//467-474 | non occupare questo campo
        public decimal VALORE_MERCE { get; set; }//486-498 | 4 decimali
        public int ID_IVA { get; set; }//500-503 | ??
        public string TIPO_DATA_CONSEGNA_TASSATIVA { get; set; }//516-516 | Vuota = Tassativa il | E = Entro il | D = Dopo il
        public DateTime DATA_CONSEGNA_TASSATIVA { get; set; }//517-524 | YYYYMMDD
        public DateTime DATA_APPUNTAMENTO { get; set; }//525-532 | YYYYMMDD
        public int NUMERO_BORDERO { get; set; }//533-538 | BORDERO XCM
        public DateTime DATA_BORDERO { get; set; }//539-546 | YYYYMMDD
        public string NOTE_SPEDIZIONI { get; set; }//550-749
        public string TELEFONO_DESTINATARIO { get; set; }//750-784
        public string EMAIL_DESTINATARIO { get; set; }//785-849
        public string MISURE_COLLI { get; set; }//850-1049
        public decimal PESO_ADR_PRINCIPALE { get; set; }//1050-1057 | 3 decimali  | INFORMARSI ADR FARMACO
        public int COLLI_ADR_PRINCIPALE { get; set; }//1058-1062 | Numero della classe di merce pericolosa (2,3,4.1,4.2,4.3,4.1,5.2,8,9)
        public string TIPO_ADR_SECONDARIO { get; set; }//1063-1067
        public decimal PESO_ADR_SECONDARIO { get; set; }//1068-1075 | 3 decimali
        public int COLLI_ADR_SECONDARIO { get; set; }//1076-1080 | Numero della classe di merce pericolosa (2,3,4.1,4.2,4.3,4.1,5.2,8,9)
        public string TIPO_ADR_EVEN_TERZA_TIPOLOGIA { get; set; }//1063-1067
        public decimal PESO_ADR_EVEN_TERZA_TIPOLOGIA { get; set; }//1068-1075 | 3 decimali
        public int COLLI_ADR_EVEN_TERZA_TIPOLOGIA { get; set; }//1076-1080 | Numero della classe di merce pericolosa (2,3,4.1,4.2,4.3,4.1,5.2,8,9)
        public InterpreteInvioCDLColli[] COLLI_ALLEGATI { get; set; }// Array generato per l'esplosione del dettaglio colli
    }
    public class InterpreteInvioCDLColli
    {
        public string BARCODE_BOLLA { get; set; }//1-14
        public string VUOTO { get; set; }//15-15
        public int ESERCIZIO { get; set; }//16-19 YYYY
        public int ID_CLIENTE { get; set; }//20-27 | Fornito dal cliente di consult
        public int NUMERO_COLLI { get; set; }//28-32
        public int BANCALI { get; set; }//33-37
        public int PRIMO_SEGNACOLLO { get; set; }//38-45
        public string TIPO_SEGNACOLLO { get; set; }//46-47
        public int ULTIMO_SEGNACOLLO { get; set; }//48-55
        public string BARCODE_SEGNACOLLO { get; set; }//56-85
        public int NUMERO_DEL_COLLO { get; set; }//86-89
        public DateTime DATA_E_ORA_CREAZIONE { get; set; }//90-104 | ddmmyyyy hhmmss
        public DateTime DATA_E_ORA_AGGIORNAMENTO { get; set; }//105-119
    }
}