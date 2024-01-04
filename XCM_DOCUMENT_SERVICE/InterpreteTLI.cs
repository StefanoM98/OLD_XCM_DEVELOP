using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCM_DOCUMENT_SERVICE
{
    //POS - LUN
    public class InterpreteTLIBolle
    {
        public string ANNUL_MANUT_LOGICO { get; set; }//1-1 | A | Campo da non gestire
        public int TIPO_BOLLA { get; set; }//2-1 | A | 0 = conto servizio 1 = porto franco  2 = franco contro anticipata 3 = nolo prepagato 4 = porto franco fino a 5 = tassato destino 6 = porto assegnato 7 = assegnato conto corrente
        public DateTime DATA_BOLLA { get; set; }//3-8 | S | AAAAMMGG
        public string XAB_CLIENTE { get; set; }//11-20 | A | Riferimento del documente del cliente mittente
        public string RIF_MITTENTE2 { get; set; }//31-20 | A | Eventuale secondo riferimento del mittente
        public string RIF_OPERATORE_LOGISTICO { get; set; }//51-20 | A | Riferimento dell'operatore logistico precedente
        public DateTime DATA_XAB { get; set; }//91-8 | S | Data del documento del cliente mittente
        public string ANNO_BOLLETTAZIONE { get; set; }//79-2 | S |  chiave di spedizione e obbligatoriamente riportati Nell’eventuale file esiti di ritorno
        public string FILIALE_BOLLETTAZIONE { get; set; }//81-2 | A | chiave di spedizione e obbligatoriamente riportati Nell’eventuale file esiti di ritorno
        public string PROGRESSIVO_CHIAVE { get; set; }//83-7 | S | chiave di spedizione e obbligatoriamente riportati Nell’eventuale file esiti di ritorno
        public string CORR_BORD { get; set; }//90-5 | A | Codice del corrispondente  ???
        public DateTime DATA_BORDERO { get; set; }//95-8 | S | Data del borderò di affidamento
        public string FILIALE { get; set; }//103-2 | A | identifica il cliente mittente se codificato 
        public string CODICE_CLIENTE { get; set; }//105-6 | S | identifica il cliente mittente se codificato 
        public string MITTENTE_CONTRATTO { get; set; }//111-3 | S | identifica il cliente mittente se codificato 
        public string RAGIONE_SOCIALE { get; set; }//114-30 | A | riferito al mittente della spedizione
        public string INDIRIZZO { get; set; }//144-30 | A | riferito al mittente della spedizione
        public string LOCALITA { get; set; }//174-30 | A | riferito al mittente della spedizione
        public string SIGLA_PART_DEST { get; set; }//204-5 | A | riferito al mittente della spedizione
        public string CAP { get; set; }//209-8 | A | riferito al mittente della spedizione
        public string STATO_MITTENTE { get; set; }//217-4 | A | inteso come nazione???
        public string MAGAZZINO_DI_PARTENZA { get; set; }//221-3 | A | E’ impostato il codice magazzino di partenza (BOLEG). ?????
        public string RAGIONE_SOCIALE_DESTINAZIONE { get; set; }//224-30 | A |
        public string INDIRIZZO_DESTINAZIONE { get; set; }//254-30 | A |
        public string LOCALITA_DESTINAZIONE { get; set; }//284-30 | A |
        public string SIGLA_PARTENZA_DESTINAZIONE { get; set; }//314-5 | A | ???
        public string CAP_DESTINAZIONE { get; set; }//319-8 | A |
        public string CODICE_AEREOPORTO_DESTINAZIONE { get; set; }//331-3 | A | ???
        public string CODICE_MERCE { get; set; }//334-3 | A | ??
        public string CODICE_MEZZO { get; set; }//337-15 | A | ??
        public string NUMERO_BANCALI { get; set; }//352-5 | A | con che formattazione???
        public decimal PESO { get; set; }//357-7 | S | 2 decimali (e le restanti 5 cifre con 0? va espressa la virgola?)
        public string COLLI { get; set; }//364-5 | S | con che formattazione???
        public decimal METRI_CUBI { get; set; }//369-5 | A | 2 decimali (e le restanti cifre? va espressa la virgola?)
        public string FASCIA_SEGNACOLLI { get; set; }//374-3 | S | ???
        public string SEGNACOLLO_DAL { get; set; }//377-5 | S | ??? segnacollo iniziale
        public string SEGNACOLLO_AL { get; set; }//382-5 | S | ??? segnacollo finale
        public string SEGNACOLLI_DIVERSI { get; set; }//387-1 | A | ??? Impostato il valore “1” se ci sono segnacolli a dettaglio. 
        public decimal IMPORTO_CONTRASSEGNO { get; set; }//388-13 | S | 3 decimali (e le restanti cifre? va espressa la virgola?)
        public string DIVISA_CONTRASSEGNO { get; set; }//401-3 | A | VALUTA
        public string IMPORTO_ANTICIPATA { get; set; }//404-13 | S | 3 decimali anticipo importo del contrassegno 
        public string DIVISA_ANTICIPATA { get; set; }//417-3 | A |
        public string NUMERO_FATTURA_X_ANTICIPATA { get; set; }//420-7 | A | 
        public string CODICE_VETTORE_X_ANTICIPATA { get; set; }//427-5 | S |
        public string TOT_FATTURA_X_P_ASS { get; set; }//432-13 | S | 3 decimali ???
        public string DIV_FATT_X_P_ASSEG { get; set; }//445-3 | A | ???
        public string TIPO_FATT_X_P_ASSEG { get; set; }//448-1 | A | ???
        public string NUMERO_FATT_X_P_ASS { get; set; }//449-7 | S | ???
        public DateTime DATA_FATT_X_P_ASS { get; set; }//456-8 | S | ??? che formato
        public string VETTORE_RITIRO { get; set; }//464-5 | A | S | ???
        public DateTime DATA_RITIRO_MERCE { get; set; }//469-8 | S | ??? che formato
        public int TIPO_SERVIZIO { get; set; }//477-1 | A | Tipo di servizio per la spedizione i valori sono in una specifica tabella.
        public string GIORNO_CHIUSURA { get; set; }//478-3 | A | Se impostato deve contenere i seguenti valori LUN – MAR – MER – GIO – VEN – SAB - DOM
        public string TEST_GIORNO_CHIUSURA { get; set; }//481-1 | A | I valori possibili sono “1” mattina “2” pomeriggio “3” tutto il giorno
        public DateTime DATA_CONSEGNA_TASSATIVA { get; set; }//482-8 | S | E’ la data per la consegna e può avere più significati in funzione del test seguente. 
        public int TIPO_CONSEGNA { get; set; }//490-1 | A | I valori possibili sono : “blank” = prevista consegna “1” = tassativa consegna “2” = di prenotazione “3” = entro il “4” = a partire dal “5” = tassativa consegna + blocco spedizione
        public string NOTE_CONSEGNA { get; set; }//491-40 | A |
        public string UFFICIO_ESECUTIVO { get; set; }//531-1 | A | Non impostato
        public string TEST_LIBERO_2 { get; set; }//532-1 | A | 
        public string TEST_LIBERO_3 { get; set; }//533-1 | A | 
        public string TEST_LIBERO { get; set; }//534-1 | A | 
        public string TEST_LIBERO_5 { get; set; }//535-1 | A | 
        public string ANNO_BORDERO { get; set; }//536-2 | S | che formato??? *Nei tre campi sopra descritti viene registrata la chiave del viaggio. 
        public int FILIALE_EMISSIONE_BORDERO { get; set; }//538-2 | A | *Nei tre campi sopra descritti viene registrata la chiave del viaggio. 
        public string KEY_BORDERO { get; set; }//540-7 | S | *Nei tre campi sopra descritti viene registrata la chiave del viaggio. 
        public string FILIALE_DI_PARTENZA { get; set; }//547-2 | A | ??? 
        public string ZONA { get; set; }//549-5 | A | ???
        public string FILIALE_PARTENZA { get; set; }//547-2 | A | ???
        public string SOC_COMPETENZA { get; set; }//556-2 | A | ???
        public decimal IMPORTO_VALORE_MERCE { get; set; }//558-13 | S | 3 decimali (e le restanti cifre? va espressa la virgola?)
        public string DIVISA_VALORE_MERCE { get; set; }//571-3 | A | valuta 
    }

    public class InterpreteTLIEsiti
    {
        public int ANNUL_MANUT_LOGICO { get; set; }//1-1 | A | Campo da non gestire
        public string UTENTE { get; set; }//2-10 | A | Campo da non gestire
        public string ANNO_TRACKING { get; set; }//12-2 | S | Campo da non gestire
        public string FILIALE_TRACKING { get; set; }//14-2 | A | Campo da non gestire
        public string NUMERO_TRACKING { get; set; }//16-7 | S | Campo da non gestire
        public DateTime DATA_EVENTO { get; set; }//23-8 | S | Data evento in AAAAMMGG 
        public string ORA_EVENTO { get; set; }//31-6 | S | Ora evento in HHMMSS 
        public string TIPO_EVENTO { get; set; }//37-2 | A | ??? I due campi sopra descritti contengono il tipo e codice evento originale.
        public string CODICE_EVENTO { get; set; }//39-3 | A | ??? I due campi sopra descritti contengono il tipo e codice evento originale.
        public string TIPO_DETTAGLIO { get; set; }//42-1 | S | ???
        public string ANNO_DETTAGLIO { get; set; }//43-2 | S | Campo da non gestire
        public string FILIALE_DETTAGLIO { get; set; }//45-2 | A | Campo da non gestire
        public string KEY_DETTAGLIO { get; set; }//47-7 | S | Campo da non gestire
        public string CTR_X_GEST_COMPLETI { get; set; }//54-3 | S | Campo da non gestire
        public string FILIALE_COMPETENTE { get; set; }//57-2 | A | Campo da non gestire
        public string FILIALE_CLIENTE { get; set; }//59-2 | A | Campo da non gestire
        public string CODICE_CLIENTE { get; set; }//61-6 | S | Campo da non gestire
        public string RIFERIMENTO_CLIENTE { get; set; }//67-20 | A | Riferimento del cliente mittente
        public string RIFERIMENTO_CORRISPONDENTE { get; set; }//87-20 | A | Riferimento dell'operatore logistico precedente (corrispondente)
        public string ANNO_DOCUMENTO { get; set; }//107-4 | S | I quattro campi sopra descritti se utilizzati devono contengono i dati di fattura.
        public string FILIALE_DOCUMENTO { get; set; }//111-2 | A | ??? I quattro campi sopra descritti se utilizzati devono contengono i dati di fattura.
        public string TIPO_DOCUMENTO { get; set; }//113-1 | A | ??? I quattro campi sopra descritti se utilizzati devono contengono i dati di fattura.
        public string NUMERO_DOCUMENTO { get; set; }//114-7 | S | ??? I quattro campi sopra descritti se utilizzati devono contengono i dati di fattura.
        public string ANNO_DISTRIBUZIONE { get; set; }//121-2 | S | I tre campi sopra descritti contengono i riferimenti al giro di distribuzione
        public string FILIALE_DISTRIBUZIONE { get; set; }//123-2 | I tre campi sopra descritti contengono i riferimenti al giro di distribuzione
        public string NUMERO_DISTRIBUZIONE { get; set; }//125-7 | S | I tre campi sopra descritti contengono i riferimenti al giro di distribuzione
        public string DIVISA_IMPORTO { get; set; }//140-3 | A | valuta
        public decimal IMPORTO { get; set; }//143-13 | S | 3 decimali (va espressa la virgola?) I tre campi sopra descritti riguardano importi di incasso contrassegni , anticipate o porti assegnati.
        public decimal RETTIFICA_IMPORTO { get; set; }//156-13 | S | 3 decimali (va espressa la virgola?) I tre campi sopra descritti riguardano importi di incasso contrassegni , anticipate o porti assegnati.
        public string NUMERO_COLLI { get; set; }//169-5 | S | (e le restanti cifre?)
        public decimal PESO { get; set; }//174-7 | S | 2 decimali (va espressa la virgola?)
        public string DESCRIZIONE { get; set; }//181-30 | A | Descrizione aggiuntiva all'evento
        public string ANNO_RIFERIMENTO_TRACK { get; set; }//211-2 | S | Campo da non gestire
        public string FILIALE_RIFERIMENTO_TRACK { get; set; }//213-2 | A | Campo da non gestire
        public string NUMERO_RIFERIMENTO_TRACK { get; set; }//215-7 | S | Campo da non gestire
        public string CTR_X_GEST_COMPLETI2 { get; set; }//222-3 | S | Campo da non gestire già presente in posizione 54-3
        public string TRASFERIMENTO_SEDE { get; set; }//225-1 | A | Campo da non gestire
        public string TRASFERIMENTO_FILIALE { get; set; }//226-1 | A | Campo da non gestire
        public string TRASFERIMENTO_IN_BANCA_DATI { get; set; }//227-1 | A | Campo da non gestire
        public string GESTIONE_MANCANZA { get; set; }//228-1 | A | Campo da non gestire
        public string GESTIONE_PRATICA_MANCANZA { get; set; }//229-1 | A | Campo da non gestire
        public string GESTIONE_DANNI_CONTENZIOSO { get; set; }//230-1 | A | Campo da non gestire
        public string GESTIONE_PRATICA_DANNI { get; set; }//231-1 | A | Campo da non gestire
        public string GESTIONE_GIACENZE { get; set; }//232-1 | A | Campo da non gestire
        public string GESTIONE_C_ASSEGNI { get; set; }//233-1 | A | Campo da non gestire
        public string GESTIONE_ANTICIPATA { get; set; }//234-1 | A | Campo da non gestire
        public string GESTIONE_BORDERO_GIRI { get; set; }//235-1 | A | Campo da non gestire
        public string GESTIONE_DETTAGLIO_SEGNACOLLI { get; set; }//236-1 | A | Campo da non gestire
        public string GESTIONE_ECCEDENZA { get; set; }//237-1 | A | Campo da non gestire
        public string GESTIONE_PRATICHE_ECCEDENZA { get; set; }//238-1 | A | Campo da non gestire
        public string GESTIONE_PORTI_ASSEGNATI { get; set; }//239-1 | A | Campo da non gestire
        public string TEST_16 { get; set; }//240-1 | A | Campo da non gestire
        public string TEST_17 { get; set; }//241-1 | A | Campo da non gestire
        public string TEST_18 { get; set; }//242-1 | A | Campo da non gestire
        public string TEST_19 { get; set; }//243-1 | A | Campo da non gestire
        public string TEST_20 { get; set; }//244-1 | A | Campo da non gestire
        public string TEST_21 { get; set; }//245-1 | A | Campo da non gestire
        public string TEST_22 { get; set; }//246-1 | A | Campo da non gestire
        public string TEST_23 { get; set; }//247-1 | A | Campo da non gestire
        public string TEST_24 { get; set; }//248-1 | A | Campo da non gestire
        public string TEST_25 { get; set; }//249-1 | A | Campo da non gestire
        public string CODICE_X_CLIENTE { get; set; }//250-10 | A | Contiene il codice evento usabile dal cliente. I valori di questo campo devono essere caricati in una Tabella che deve essere concordata tra il trasportatore e il cliente.
        public string LIBERO_1 { get; set; }//260-10 | A | Campo da non gestire
        public string LIBERO_2 { get; set; }//270-10 | A | Campo da non gestire
        public string LIBERO_3 { get; set; }//280-10 | A | Campo da non gestire
        public string LIBERO_4 { get; set; }//290-10 | A | Campo da non gestire
        public DateTime DATA_DDT_CLIENTE { get; set; }//300-8 | S | Contiene la data del documento del cliente DDT

        ///NOTE 
        ///• Tutti i campi definiti “S” devono essere riempiti di zeri quando non utilizzati.
        ///• Tutti campi data sono scritti in AAAAMMGG.
    }


}
