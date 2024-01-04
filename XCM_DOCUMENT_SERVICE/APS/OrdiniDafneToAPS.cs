using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCM_DOCUMENT_SERVICE.APS
{
    public class OrdiniDafneToAPS
    {
        string quote = "\"";
        public string OR_STATO { get; set; }//Stato ordine - N - L6
        public string ORSERIAL { get; set; }//ORSERIAL - ID - C - L50
        public string ORTIPDOC { get; set; }//ORTIPDOC - Tipo Documento - C - L2
        public string ORNUMDOC { get; set; }//ORNUMDOC - Numero Documento - N - L15
        public string ORALFDOC { get; set; }//ORALFDOC - Alfa Documento - C - L10
        public string ORDATDOC { get; set; }//ORDATDOC - Data Documento - D - L8
        public string ORNUMEST { get; set; }//ORNUMEST - N. Rif. Documento - N - L15
        public string ORALFEST { get; set; }//ORALFEST - Alfa rif. esterno - C - L10
        public string ORDATEST { get; set; }//ORDATEST - Data rif. esterno - C - L8
        public string ORTIPCON { get; set; }//ORTIPCON - Tipo conto C/F - C - L1
        public string ORCODCON { get; set; }//ORCODCON - Codice cli/for - C - L15
        public string ORCODAGE { get; set; }//ORCODAGE - Codice agente - C - L5
        public string ORCODDES { get; set; }//ORCODDES - Codice destinazione - C - L5
        public string DDNOMDES { get; set; }//
        public string DDINDIRI { get; set; }//
        public string DD__CAP { get; set; }//
        public string DDLOCALI { get; set; }//
        public string DDPROVIN { get; set; }//
        public string DDCODNAZ { get; set; }//
        public string ORCODPAG { get; set; }//ORCODPAG - Codice pagamento - C - L5
        public string MVDESDOC { get; set; }//
        public string ORCODVAL { get; set; }//ORCODVAL - Codice valuta - C - L3
        public string ORSCOCL1 { get; set; }//ORSCOCL1 - 1 Sconto - N - L6|2dec
        public string ORSCOCL2 { get; set; }//ORSCOCL2 - 2 Sconto - N - L6|2dec
        public string ORSCOPAG { get; set; }//ORSCOPAG - Sconto su pagamento - N - L6|2dec
        public string ORFLSCOR { get; set; }//ORFLSCOR - Test scorporo Piede - C - L1
        public string ORSCONTI { get; set; }//ORSCONTI - Totale sconti e maggiorazioni - N - L18|4dec
        public string ORTDTEVA { get; set; }//ORTDTEVA - Data evasiione di testata - D - L8
        public string ORSPEINC { get; set; }//ORSPEINC - Spese incasso - N - L18|4dec
        public string ORSPETRA { get; set; }//ORSPETRA - Spese di trasporto - N - L18|4dec
        public string ORSPEIMB { get; set; }//ORSPEIMB - Spese imballo - M - L18|4dec
        public string ORCODSPE { get; set; }//ORCODSPE - Codice spedizione - C - L3 
        public string ORCODVET { get; set; }//ORCODVET - Codice vettore - C - L5
        public string ORCODPOR { get; set; }//ORCODPOR - Codice porto - C - L1
        public string ORCONCON { get; set; }//ORCONCON - Condizioni di consegna - C - L1
        public string UTCC { get; set; }//UTCC - Inserito da - N - L4
        public string UTDC { get; set; }//UTDC - Data creazione - T - L14
        public string UTCV { get; set; }//UTCV - Variato da - T - L4
        public string UTDV { get; set; }//UTDV - Data variazione - T - L14
        public string OR__NOTE { get; set; }//OR__NOTE - Note - M - L10
        public string ORMERISP { get; set; }//ORMERISP - Messaggio di risposta - M - L10
        public string ORTOTORD { get; set; }//ORTOTORD - Importo totale ordine - N - L18|5dec
        public string OR_EMAIL { get; set; }//OR_EMAIL - Indirizzo e-mail per invio comunicazioni - M - L10
        public string ORFLMAIL { get; set; }//ORFLMAIL - Flag per invio e-mail di conferma - C - L1
        public string ORNETMER { get; set; }//ORNETMER - Totale netto merce - N - L18|5dec
        public string ORLOGSTM { get; set; }//ORLOGSTM - Logistica store - C - L5
        public string ORRIFEST { get; set; }//ORRIFEST - Riferimento esterno - C - L40
        public string ORVALNAZ { get; set; }//ORVALNAZ - Cod. valuta nazionale - C - L3
        public string ORCAOVAL { get; set; }//ORCAOVAL - Cambio - N - L12|7dec
        public string ORACCONT { get; set; }//ORACCONT - Acconto contestuale - N - L18|5dec
        public string ORVALACC { get; set; }//ORVALACC - Codice valuta acconto - C - L3
        public string ORSPEBOL { get; set; }//ORSPEBOL - Spese bolli - N - L18|5dec
        public string CPROWNUM { get; set; }//
        public string ORCODICE { get; set; }//ORCODICE - Chiave di ricerca - C - L41
        public string CPROWORD { get; set; }//CPROWORD - Numero riga per  - L5
        public string ORCODART { get; set; }//ORCODART - Codice articolo - C - L20
        public string ORCODVAR { get; set; }//ORCODVAR - Codice variante - C - L20
        public string ORDESART { get; set; }//ORDESART - Descrizione - C - L20
        public string ORTIPRIG { get; set; }//ORTIPRIG - Tipo riga - C - L1
        public string ORDESSUP { get; set; }//ORDESSUP - Descrizione aggiuntiva - M - L10
        public string ORUNIMIS { get; set; }//ORUNIMIS - UM - C - L3
        public string ORQTAMOV { get; set; }//ORQTAMOV - Qta movimentata - N - L12|3dec
        public string ORQTAUM1 { get; set; }//ORQTAUM1 - Quantita movimentata rispet..  - N - L12|3dec
        public string ORPREZZO { get; set; }//ORPREZZO - Prezzo unitario  - N - L18|5dec
        public string ORSCONT1 { get; set; }//ORSCONT1 - 1 sconto - N - L6|2dec
        public string ORSCONT2 { get; set; }//ORSCONT2 - 2 sconto - N - L6|2dec
        public string ORSCONT3 { get; set; }//ORSCONT3 - 3 sconto - N - L6|2dec
        public string ORSCONT4 { get; set; }//ORSCONT4 - 4 sconto - N - L6|2dec
        public string ORSCOIN1 { get; set; }//1 sconto iniziale determina.. - N - L6|2dec
        public string ORSCOIN2 { get; set; }//2 sconto iniziale determina.. - N - L6|2dec
        public string ORSCOIN3 { get; set; }//3 sconto iniziale determina.. - N - L6|2dec
        public string ORSCOIN4 { get; set; }//4 sconto iniziale determina.. - N - L6|2dec
        public string ORCODIVA { get; set; }//ORCODIVA - Codice IVA (articolo) - C - L5
        public string ORFLOMAG { get; set; }//ORFLOMAG - Flag omaggio/sc.merce - C - L1
        public string ORDATEVA { get; set; }//ORDATEVA - Data evasione prevista - D - L8
        public string ORCODLIS { get; set; }//ORCODLIS - Codice listino - C - L
        public string ORCONTRA { get; set; }//ORCONTRA - Codice contratto - C
        public string ORRIFKIT { get; set; }//ORRIFKIT - Rif. riga kit (se articolo com..) - N
        public string ORLOGSTD { get; set; }//ORLOGSTD - Logical store - C
        public string ORUMNODI { get; set; }//ORUMNODI - UnitPriceNoDiscount - N - 20|6dec
        public string ORVALSCO { get; set; }//ORVALSCO - Importo sconto - N | 18|5dec

        public override string ToString()
        {
            return $"{OR_STATO},{quote}{ORSERIAL}{quote},{quote}{ORTIPDOC}{quote},{ORNUMDOC},{quote}{ORALFDOC}{quote},{ORDATDOC},{ORNUMEST}," +
                $"{quote}{ORALFEST}{quote},{quote}{ORDATEST}{quote},{quote}{ORTIPCON}{quote},{quote}{ORCODCON}{quote},{quote}{ORCODAGE}{quote}," +
                $"{quote}{ORCODDES}{quote},{quote}{DDNOMDES}{quote},{quote}{DDINDIRI}{quote},{quote}{DD__CAP}{quote},{quote}{DDLOCALI}{quote}," +
                $"{quote}{DDPROVIN}{quote},{quote}{DDCODNAZ}{quote},{quote}{ORCODPAG}{quote},{quote}{MVDESDOC}{quote},{quote}{ORCODVAL}{quote}," +
                $"{ORSCOCL1},{ORSCOCL2},{ORSCOPAG},{quote}{ORFLSCOR}{quote},{ORSCONTI},{quote}{ORTDTEVA}{quote},{ORSPEINC},{ORSPETRA},{ORSPEIMB}," +
                $"{quote}{ORCODSPE}{quote},{quote}{ORCODVET}{quote},{quote}{ORCODPOR}{quote},{quote}{ORCONCON}{quote},{quote}{UTCC}{quote}," +
                $"{quote}{UTDC}{quote},{quote}{UTCV}{quote},{quote}{UTDV}{quote},{quote}{OR__NOTE}{quote},{quote}{ORMERISP}{quote},{ORTOTORD}," +
                $"{quote}{OR_EMAIL}{quote},{quote}{ORFLMAIL}{quote},{ORNETMER},{quote}{ORLOGSTM}{quote},{quote}{ORRIFEST}{quote},{quote}{ORVALNAZ}{quote}," +
                $"{ORCAOVAL},{ORACCONT},{quote}{ORVALACC}{quote},{ORSPEBOL},{CPROWNUM},{quote}{ORCODICE}{quote},{CPROWORD},{quote}{ORCODART}{quote}," +
                $"{quote}{ORCODVAR}{quote},{quote}{ORDESART}{quote},{quote}{ORTIPRIG}{quote},{quote}{ORDESSUP}{quote},{quote}{ORUNIMIS}{quote},{ORQTAMOV}," +
                $"{ORQTAUM1},{ORPREZZO},{ORSCONT1},{ORSCONT2},{ORSCONT3},{ORSCONT4},{ORSCOIN1},{ORSCOIN2},{ORSCOIN3},{ORSCOIN4},{quote}{ORCODIVA}{quote}," +
                $"{quote}{ORFLOMAG}{quote},{ORDATEVA},{quote}{ORCODLIS}{quote},{quote}{ORCONTRA}{quote},{ORRIFKIT}{quote}{ORLOGSTD}{quote},{ORUMNODI},{ORVALSCO}";
        }
    }

    public class RispostaEvasioneDDTAPS
    {
        string quote = "\"";
        public string MVSERIAL { get; set; }//stringa
        public string MVDATDOC { get; set; }//datatime GG/MM/AAAA
        public string MVNUMDOC { get; set; }
        public string MVALFDOC { get; set; }//stringa
        public string MVCODCLI { get; set; }//stringa
        public string MVCODART { get; set; }//stringa
        //public string CPROWORD { get; set; }
        public string MVDESART { get; set; }//stringa primi 40 caratteri descrizione
        public string MVDESSUP { get; set; }//stringa successivi 40 caratteri
        public string MVQTAMOV { get; set; }
        public string MVCODLOT { get; set; }//stringa
        public string MVCODMAT { get; set; }//stringa
        public string CPROWNUM { get; set; }

        public override string ToString()
        {
            return $"{MVDATDOC},{MVNUMDOC},{quote}{MVALFDOC}{quote},{quote}{MVCODCLI}{quote},{quote}{MVSERIAL}{quote},{CPROWNUM},{quote}{MVCODART}{quote},{quote}{MVDESART}{quote},{quote}{MVDESSUP}{quote},{MVQTAMOV},{quote}{MVCODLOT}{quote},{quote}{MVCODMAT}{quote}";
        }

    }
}
