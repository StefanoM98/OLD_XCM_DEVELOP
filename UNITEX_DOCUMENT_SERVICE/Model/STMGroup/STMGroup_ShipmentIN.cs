using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNITEX_DOCUMENT_SERVICE.Model.STMGroup
{
    public class STMGroup_ShipmentIN
    {
        //NOTE
        //•	Tutti i campi definiti “S” devono essere riempiti di zeri quando non utilizzati.
        //•	Tutti campi data sono scritti in AAAAMMGG.


        //ANNUL/MANUT. LOGICO - Campo da non gestire
        public string DKREC { get; set; }
        public int[] idxDKREC = new int[] { 0, 1 };

        //TIPO BOLLA Verrà gestito il tipo spedizione “1” porto franco o “2” franco contro anticipata in automatico dalla M.C.M Se usato deve esistere nella relativa tabella di correlazione.- Campo da non gestire
        public string TipoBolla { get; set; }
        public int[] idxDKTIP = new int[] { 1, 1 };

        //(S) DATA BOLLA AAAAMMGG
        public string DataBollaAAAAAMMGG { get; set; }
        public int[] idxDKDTB = new int[] { 2, 8 };

        //Diventa la data di spedizione se impostato il relativo test nella tabella T DKT. DKRMI XAB CLIENTE
        public string RifInterno { get; set; }
        public int[] idxDKRMI = new int[] { 10, 20 };

        //Campo facoltativo
        public string DKRM2 { get; set; }
        public int[] idxDKRM2 = new int[] { 30, 20 };

        //RIF.OPE.LOGISTICO E’ il riferimento dell’operatore logistico QUESTO RIFERIMENTO DEVE ESSERE USATO NEL FILE ESITI e servirà a STM come chiave univoca.
        public string RiferimentoOperatoreLogistico { get; set; }
        public int[] idxDKRUL = new int[] { 50, 20 };

        //(S) DATA XAB AAAAMMGG
        public string DataXAB_AAAAMMGG { get; set; }
        public int[] idxDKDTX = new int[] { 70, 8 };

        //I tre campi DKANB,DKFBO,DKKEY possono indicare la chiave di spedizione dell’operatore logistico precedente
        //(S) ANNO BOLLETTAZIONE
        public string AnnoBollettazione { get; set; }//DKANB
        public int[] idxDKANB = new int[] { 78, 2 };

        //FIL. BOLLETTAZIONE
        public string FilialeBollettazione { get; set; }
        public int[] idxDKFBO = new int[] { 80, 2 };

        //(S) PROGRESSIVO CHIAVE
        public string ProgressivoChiave { get; set; }
        public int[] idxDKKEY = new int[] { 82, 7 };

        //CODICE CORRIERE
        public string CodiceCorriere { get; set; }//DKBOR
        public int[] idxDKBOR = new int[] { 89, 5 };

        //(S) RAGGRUPPAMENTO
        public string Raggruppamento { get; set; }//DKDBO
        public int[] idxDKDBO = new int[] { 94, 8 };

        //I due campi Filiale (DKMFI) e codice cliente (DKMCO) vengono impostati dalla tabella ricezione dati.
        //È possibile gestirli sul File di input , in questo caso deve essere compilato lo specifico test con il valore
        //“2” sulla tabella di ricezione dati T – DKT.

        //FILIALE - Campo da non gestire
        public string Filiale { get; set; }
        public int[] idxDKMFI = new int[] { 102, 2 };

        //(S) CODICE CLIENTE - Campo da non gestire
        public string CodiceCliente { get; set; }
        public int[] idxDKMCO = new int[] { 104, 6 };

        //(S) MITT.CONTRATTO - Codice contratto da applicare al cliente. Per dettagli vedere punto 2)
        public string ContrattoMittente { get; set; }
        public int[] idxDKMCN = new int[] { 110, 3 };


        //I cinque campi DKMIT,DKMIN,DKMLO,DKMPR,DKMCP sono riferiti al mittente della spedizione.
        //Se attivato il controllo delle località possono essere normalizzati dalla procedura M.C.M.

        //RAGIONE SOCIALE
        public string RagioneSocialeMittente { get; set; }
        public int[] idxDKMIT = new int[] { 113, 30 };

        //INDIRIZZO
        public string IndirizzoMittente { get; set; }
        public int[] idxDKMIN = new int[] { 143, 30 };

        //LOCALITA
        public string LocalitaMittente { get; set; }
        public int[] idxDKMLO = new int[] { 173, 30 };

        //SIGLA PART./DEST.
        public string DKMPR { get; set; }
        public int[] idxDKMPR = new int[] { 203, 5 };

        //CAP
        public string CapMittente { get; set; }
        public int[] idxDKMCP = new int[] { 208, 8 };

        //ORA VIAGGIO  - Se il test  DKT04=”V” questa indica l’ora del viaggio
        public string OraViaggio { get; set; }
        public int[] idxDKMST = new int[] { 216, 4 };

        //MAGAZZINO DI PARTENZA
        public string MagazzinoDiPartenza { get; set; }
        public int[] idxDKMAE = new int[] { 220, 3 };


        //I cinque campi DKDIT,DKDIN,DKDLO,DKDPR,DKDCP sono riferiti al destinatario della spedizione.
        //Se attivato il controllo delle località possono essere normalizzati dalla procedura M.C.M

        //RAGIONE SOCIALE DEST.
        public string RagioneSocialeDestinatario { get; set; }
        public int[] idxDKDIT = new int[] { 223, 30 };

        //INDIRIZZO DEST.
        public string IndirizzoDestinatario { get; set; }
        public int[] idxDKDIN = new int[] { 253, 30 };

        //LOCALITA' DEST.
        public string LocalitaDestinatario { get; set; }
        public int[] idxDKDLO = new int[] { 283, 30 };

        //SIGLA PART./DEST.
        public string SiglaPartDest { get; set; }
        public int[] idxDKDPR = new int[] { 313, 5 };

        //C.A.P. DEST.
        public string CapDestinatario { get; set; }//DKDCP
        public int[] idxDKDCP = new int[] { 318, 8 };

        //ORA CONSEGNA - Campo facoltativo
        public string OraConsegna { get; set; }
        public int[] idxDKDST = new int[] { 326, 4 };

        //LIBERO
        public string DKDAE { get; set; }//DKDAE
        public int[] idxDKDAE = new int[] { 330, 3 };

        //LIBERO - Verrà gestito in base al corrispondente test della tabella T-DKT.
        public string CampoLibero { get; set; }//DKCNM
        public int[] idxDKCNM = new int[] { 333, 3 };

        //I primi 11 caratteri verranno riportati nel campo automezzo nelle spedizioni (BOAUT). Gli altri quattro  
        //caratteri identificano l’esistenza di codici di addebito.La ricezione di questi addebiti si attiva con un test
        //sulla tabella T-DKT e i relativi codici addebito sono identificati sull’anagrafico commerciale fornitori.

        //CODICE MEZZO 
        public string CodiceMezzo { get; set; }
        public int[] idxDKDNM = new int[] { 336, 15 };

        //(S) NUMERO BANCALI              
        public string NumeroBancali { get; set; }//DKBAN
        public int[] idxDKBAN = new int[] { 351, 5 };

        //(S) PESO - 2 decimali  
        public string Peso2Dec { get; set; }
        public int[] idxDKPES = new int[] { 356, 7 };

        //(S) COLLI                   
        public string Colli { get; set; }//DKCOL
        public int[] idxDKCOL = new int[] { 363, 5 };

        //(S) METRI CUBI - 2 decimali                      
        public string MetriCubi2Dec { get; set; }
        public int[] idxDKVOL = new int[] { 368, 5 };

        //FASCIA SEGNACOLLI - È un valore che deve essere concordato con M.C.M. per la gestione dei segnacolli con bar code.Oppure : I primi due caratteri di sinistra se riempiti identificano l’unità di misura e verranno gestiti se il campo
        public string FasciaSegnacolli { get; set; }//DKCOD
        public int[] idxDKCOD = new int[] { 373, 3 };

        //(S) SEGNACOLLO DAL                                           
        public string SegnacolloDal { get; set; }
        public int[] idxDKSED = new int[] { 376, 5 };

        //(S) SEGNACOLLO AL                                           
        public string SegnacolloAl { get; set; }
        public int[] idxDKSEA = new int[] { 381, 5 };

        //Impostare il valore “1” se ci sono segnacolli a dettaglio
        //Impostare il valore “2” se gestiti i campi quantità unità di misura
        //Impostare il valore “3” per gestire sia i segnacolli a dettaglio e sia i campi unità di misura

        //SEGNACOLLI DIVERSI
        public string DKT01 { get; set; }
        public int[] idxDKT01 = new int[] { 386, 1 };

        //(S) IMP.CONTRASSEGNO - 3 decimali
        public string ImportoContrassegno3Dec { get; set; }
        public int[] idxDKIF1 = new int[] { 387, 13 };

        //DIVISA CONTRASSEGNO - Se impostato deve esistere nella relativa tabella di correlazione.
        public string DivisaContrassegno { get; set; }//DKDI1
        public int[] idxDKDI1 = new int[] { 400, 3 };

        //(S) IMPORTO ANTICIPATA - 3 decimali - Può esistere solo se il campo DKITF = a zero
        public string ImportoAnticipata3Dec { get; set; }
        public int[] idxDKIF2 = new int[] { 403, 13 };

        //DIVISA ANTICIPATA - Se indicato deve esistere nella tabella di correlazione divise e solo se c’è il campo DKIF2
        public string DivisaAnticipata { get; set; }
        public int[] idxDKDI2 = new int[] { 416, 3 };

        //(S) NUMERO ANTICIPATA - Numero della fattura vettore ( anticipata ) se la spedizione è tipo 2 = Franco contro. Solo per spedizioni Provenienti dal mondo M.C.M.
        public string NumeroAnticipata { get; set; }
        public int[] idxDKFTA = new int[] { 419, 7 };

        //CODICE VETTORE ANTICIPATA - Campo facoltativo
        public string CodiceVettoreAnticipata { get; set; }
        public int[] idxDKVE1 = new int[] { 426, 5 };

        //(S) TOT.FATTURA X P/ASS. - 3 decimali - Se maggiore di zero diventerà l’anticipata e verrà impostato il tipo spedizione “2” franco contro.
        public string DKITF { get; set; }
        public int[] idxDKITF = new int[] { 431, 13 };

        //TOT.FATTURA X P/ASS. - Se impostato deve esistere nella relativa tabella di correlazione
        public string DKDIF { get; set; }
        public int[] idxDKDIF = new int[] { 444, 3 };

        //LIBERO - Campo da non gestire
        public string DKTPA { get; set; }
        public int[] idxDKTPA = new int[] { 447, 1 };

        //(S) N.RO FATT. X P/ASS. - Per le anticipate questo campo identifica il numero fattura del vettore.
        public string DKNRF { get; set; }
        public int[] idxDKNRF = new int[] { 448, 7 };

        //(S) DATA FATT. X P/ASS. - Per le anticipate questo campo identifica la data fattura del vettore
        public string DKDTF { get; set; }
        public int[] idxDKDTF = new int[] { 455, 8 };

        //I due campi DKDTR e DKCDR devono essere impostati se il TEST DKT04 è uguale a “S” , in caso contrario
        //bisognerà nella fase di caricamento impostare a video il riferimento al buono di ritiro(BODTR). Se il test
        //DKT04 =”V” questa data indica la data del viaggio(BOCDC).

        //CODICE VETTORE RITIRO
        public string CodiceVettoreRitiro { get; set; }//DKCDR
        public int[] idxDKCDR = new int[] { 463, 5 };

        //(S) DATA RITIRO AAAAMMGG 
        public string DataRitiro_AAAAMMGG { get; set; }
        public int[] idxDKDTR = new int[] { 468, 8 };

        //TIPO SERVIZIO - E’ da impostare solo se si desidera modificare l’automatismo attribuzione linea e zona è Vostra cura caricare valori accettabili.
        public string TipoServizio { get; set; }
        public int[] idxDKT02 = new int[] { 476, 1 };

        //GIORNO CHIUSURA - Se impostato deve contenere i seguenti valori LUN – MAR – MER – GIO – VEN – SAB - DOM
        public string GiornoChiusura { get; set; }//DKCHI
        public int[] idxDKCHI = new int[] { 477, 3 };

        //TEST GIORNO CHIUSURA - I valori possibili sono “1” mattina    “2” pomeriggio     “3” tutto il giorno
        public string DKTCH { get; set; }
        public int[] idxDKTCH = new int[] { 480, 1 };

        //(S) DATA CONS. TASSATIVA - E’ la data per la consegna e può avere più significati in funzione del test seguente.
        public string DataConsegnaTassativa { get; set; }//DKDCV
        public int[] idxDKDCV = new int[] { 481, 8 };

        //TIPO CONSEGNA - I valori possibili sono :  “blank” = prevista consegna  “1” = tassativa consegna
        //“2” = di prenotazione   “3” = entro il    “4” = a partire dal    “5” = tassativa consegna + blocco spedizione
        public string TipoConsegna { get; set; }
        public int[] idxDKT03 = new int[] { 489, 1 };

        //NOTE CONSEGNA
        public string NoteConsegna { get; set; }
        public int[] idxDKNOT = new int[] { 490, 40 };

        //TEST RITIRO - Campo facoltativo - S = effettuato ritiro merce presso il cliente. - V = data viaggio
        public string DKT04 { get; set; }
        public int[] idxDKT04 = new int[] { 530, 1 };

        //TEST ABBONAMENTO - Impostare con il valore “A” per tutte le spedizioni in abbonamento ( gestione courier ).
        public string DKT05 { get; set; }
        public int[] idxDKT05 = new int[] { 531, 1 };

        //Libero
        public string DKT06 { get; set; }
        public int[] idxDKT06 = new int[] { 532, 1 };

        //TEST  TIPO SERVIZIO - Impostare i valori “0” per il collettame e “1” per espresso.
        public string DKT07 { get; set; }
        public int[] idxDKT07 = new int[] { 533, 1 };

        //TEST NOLO FORFAIT Se diverso da BLANK dovrà contenere la lettera della varia di nolo a forfait onnicomprensivo. In questo caso il campo DKIF2 deve essere l’importo da addebitare.
        public string DKT08 { get; set; }
        public int[] idxDKT08 = new int[] { 534, 1 };

        //Nei tre campi DKABO, DKEBO, DKKBO verranno salvati i riferimenti alla presa o ritiro nel caso la spedizione vada in scarto per errori.
        //(S) ANNO RITIRO - Campo da non gestire
        public string AnnoDiRitiro { get; set; }//DKABO
        public int[] idxDKABO = new int[] { 535, 2 };

        //FIL.EMISS. RITIRO - Campo da non gestire
        public string DKEBO { get; set; }
        public int[] idxDKEBO = new int[] { 537, 2 };

        //(S) KEY  RITIRO - Campo da non gestire
        public string DKKBO { get; set; }
        public int[] idxDKKBO = new int[] { 539, 7 };

        //UFFICIO ESECUTIVO
        public string DKFPA { get; set; }
        public int[] idxDKFPA = new int[] { 546, 2 };

        //CODICE LINEA - Se impostato diventa il codice di linea da forzare in bolla. - Campo da non gestire
        public string DKZON { get; set; }
        public int[] idxDKZON = new int[] { 548, 5 };

        //FIL.COMPET. - Campo da non gestire
        public string DKFIL { get; set; }
        public int[] idxDKFIL = new int[] { 553, 2 };

        //SOC.COMPET.  - Campo da non gestire
        public string DKSOC { get; set; }
        public int[] idxDKSOC = new int[] { 555, 2 };

        //(S) IMP. VALORE MERCE - 3 decimali
        public string ImportoValoreMerce3Dec { get; set; }
        public int[] idxDKIF3 = new int[] { 557, 13 };

        //DIVISA VALORE MERCE - Se impostato deve esistere nella relativa tabella di correlazione
        public string DivisaValoreMerce { get; set; }
        public int[] idxDKDI3 = new int[] { 570, 3 };

    }

    public class STM_EsitiOut
    {
        public string NumDDT { get; set; }
        public string DataConsegnaEffettiva { get; set; }//yyyyMMdd
        public string DataSpedizione { get; set; }//ddMMyyyy
        public string CittaDestinatario { get; set; }
        public string DataConsegnaTassativa { get; set; }//ddMMyyyy
        public string ID_Tracking { get; set; }
        public string Descrizione_Tracking { get; set; }
        public string DataTracking { get; set; }
        public string ProgressivoSpedizione { get; set; }
        public string regione { get; set; }


        public static STM_EsitiOut FromCsv(string csvLine)
        {
            var values = csvLine.Split(';');
            STM_EsitiOut stato = new STM_EsitiOut();
            stato.NumDDT = values[0];
            stato.CittaDestinatario = Convert.ToString(values[1]);
            stato.ProgressivoSpedizione = Convert.ToString(values[2]);
            return stato;
        }
    }

    public class STM_StatiDocumento
    {
        public int IdUnitex { get; set; }
        public string CodiceStato { get; set; }

        public static STM_StatiDocumento FromCsv(string csvLine)
        {
            var values = csvLine.Split(';');
            STM_StatiDocumento stato = new STM_StatiDocumento();
            stato.IdUnitex = Convert.ToInt32(values[0]);
            stato.CodiceStato = Convert.ToString(values[1]);
            return stato;

        }
    }
}
