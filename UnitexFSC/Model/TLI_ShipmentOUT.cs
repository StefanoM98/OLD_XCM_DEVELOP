using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitexFSC.Model
{
    public class TLI_ShipmentOUT
    {
        //Annullamento - Campo da non gestire
        public string DKREC { get; set; }
        public int[] idxDKREC = new int[] { 0, 1 };
        /*
         Tipo bolla:
            0 = Conto servizio
            1 = porto franco
            2 = franco contro anticipata
            3 = nolo prepagato
            4 = porto franco fino a
            5 = tassato a destino
            6 = porto assegnato
            7 = assegnato conto corrente
         */
        public string DKTIP { get; set; }
        public int[] idxDKTIP = new int[] { 1, 1 };

        //Data bolla yyyyMMdd
        public string DKDTB { get; set; }
        public int[] idxDKDTB = new int[] { 2, 8 };

        //Riferimento esterno cliente
        public string DKRMI { get; set; }
        public int[] idxDKRMI = new int[] { 10, 20 };

        //Riferimento esterno cliente 2
        public string DKRM2 { get; set; }
        public int[] idxDKRM2 = new int[] { 30, 20 };

        //Riferimento operatore logistico
        public string DKRUL { get; set; }
        public int[] idxDKRUL = new int[] { 50, 20 };

        //Data documento del cliente
        public string DKDTX { get; set; }
        public int[] idxDKDTX = new int[] { 70, 8 };

        //Anno bollettazione del cliente
        public string DKANB { get; set; }
        public int[] idxDKANB = new int[] { 78, 2 };

        //Filiale bollettazione
        public string DKFBO { get; set; }
        public int[] idxDKFBO = new int[] { 80, 2 };

        //Progressivo chiave ?? 
        public string DKKEY { get; set; }
        public int[] idxDKKEY = new int[] { 82, 7 };

        //Codice corrispondente ?? Borderò
        public string DKBOR { get; set; }
        public int[] idxDKBOR = new int[] { 89, 5 };

        //Data borderò
        public string DKDBO { get; set; }
        public int[] idxDKDBO = new int[] { 94, 8 };

        //Filiale
        public string DKMFI { get; set; }
        public int[] idxDKMFI = new int[] { 102, 2 };

        //Codice Cliente ??
        public string DKMCO { get; set; }
        public int[] idxDKMCO = new int[] { 104, 6 };

        //Mittente contratto
        public string DKMCN { get; set; }
        public int[] idxDKMCN = new int[] { 110, 3};


        //Ragione sociale mittente
        public string DKMIT { get; set; }
        public int[] idxDKMIT = new int[] { 113, 30 };

        //Indirizzo mittente
        public string DKMIN { get; set; }
        public int[] idxDKMIN = new int[] { 143, 30 };

        //Località mittente
        public string DKMLO { get; set; }
        public int[] idxDKMLO = new int[] { 173, 30 };

        //Sigla part dest. ???
        public string DKMPR { get; set; }
        public int[] idxDKMPR = new int[] { 203, 5 };

        //CAP mittente
        public string DKMCP { get; set; }
        public int[] idxDKMCP = new int[] { 208, 8 };

        //Stato mittente
        public string DKMST { get; set; }
        public int[] idxDKMST = new int[] { 216, 4 };

        //Magazzino di partenza ???
        public string DKMAE { get; set; }
        public int[] idxDKMAE = new int[] { 220, 3 };

        //Ragione sociale destinatario
        public string DKDIT { get; set; }
        public int[] idxDKDIT = new int[] { 223, 30 };

        //Indirizzo destinatario
        public string DKDIN { get; set; }
        public int[] idxDKDIN = new int[] { 253, 30 };

        //Località destinatario
        public string DKDLO { get; set; }
        public int[] idxDKDLO = new int[] { 283, 30 };

        //Sigla part dest. ???
        public string DKDPR { get; set; }
        public int[] idxDKDPR = new int[] { 313, 5 };

        //CAP destinatario
        public string DKDCP { get; set; }
        public int[] idxDKDCP = new int[] { 318, 8 };

        //Stato destinatario
        public string DKDST { get; set; }
        public int[] idxDKDST = new int[] { 326, 4 };

        //Cod aereoporto destinatario
        public string DKDAE { get; set; }
        public int[] idxDKDAE = new int[] { 330, 3 };


        //Codice merce ???
        public string DKCNM { get; set; }
        public int[] idxDKCNM = new int[] { 333, 3 };

        //Codice Mezzo ???
        public string DKDNM { get; set; }
        public int[] idxDKDNM = new int[] { 336, 15 };

        //Numero Bancali
        public string DKBAN { get; set; }
        public int[] idxDKBAN = new int[] { 351, 5 };

        //Peso 2 decimali
        public string DKPES { get; set; }
        public int[] idxDKPES = new int[] { 356, 7 };

        //Colli
        public string DKCOL { get; set; }
        public int[] idxDKCOL = new int[] { 363, 5 };

        //Volume 2 decimali
        public string DKVOL { get; set; }
        public int[] idxDKVOL = new int[] { 368, 5 };

        //Fascia segnacolli ???
        public string DKCOD { get; set; }
        public int[] idxDKCOD = new int[] { 373, 3 };

        //Segnacollo dal
        public string DKSED { get; set; }
        public int[] idxDKSED = new int[] { 376, 5 };

        //Segnacollo al
        public string DKSEA { get; set; }
        public int[] idxDKSEA = new int[] { 381, 5 };

        //Segnacolli diversi - Impostato ad 1 se ci sono segnacolli a dettaglio
        public string DKT01 { get; set; }
        public int[] idxDKT01 = new int[] { 386, 1 };

        //Importo contrassegno - 3 decimali
        public string DKIF1 { get; set; }
        public int[] idxDKIF1 = new int[] { 387, 13 };

        //Divisa contrassegno
        public string DKDI1 { get; set; }
        public int[] idxDKDI1 = new int[] { 400, 3 };

        //Importo anticipata - 3 decimali
        public string DKIF2 { get; set; }
        public int[] idxDKIF2 = new int[] { 403, 13 };

        //Divisa anticipata
        public string DKDI2 { get; set; }
        public int[] idxDKDI2 = new int[] { 416, 3 };

        //Numero fatt per anticipata
        public string DKFTA { get; set; }
        public int[] idxDKFTA = new int[] { 419, 7 };

        //Codice vettore anticipata
        public string DKVE1 { get; set; }
        public int[] idxDKVE1 = new int[] { 426, 5 };

        //Tot fattura x p/ass - 3 decimali
        public string DKITF { get; set; }
        public int[] idxDKITF = new int[] { 431, 13 };

        //Div fatt x p/asseg
        public string DKDIF { get; set; }
        public int[] idxDKDIF = new int[] { 444, 3 };

        //Tipo fattura x p/ass
        public string DKTPA { get; set; }
        public int[] idxDKTPA = new int[] { 447, 1 };

        //Numero fatt x p/ass
        public string DKNRF { get; set; }
        public int[] idxDKNRF = new int[] { 448, 7 };

        //Data fatt x p/ass
        public string DKDTF { get; set; }
        public int[] idxDKDTF = new int[] { 455, 8 };

        //Vettore ritiro
        public string DKCDR { get; set; }
        public int[] idxDKCDR = new int[] { 463, 5 };

        //Data ritiro merce
        public string DKDTR { get; set; }
        public int[] idxDKDTR = new int[] { 468, 8 };

        //Tipo servizio (i valori sono in una specifica tabella)
        public string DKT02 { get; set; }
        public int[] idxDKT02 = new int[] { 476, 1 };

        //Giorno chiusura - Se impostato deve contenere LUN - MAR - MER - GIO - VEN - SAB - DOM
        public string DKCHI { get; set; }
        public int[] idxDKCHI = new int[] { 477, 3 };

        //Test giorno di chiusura 1 = mattina, 2 = pomeriggio, 3 = tutto il giorno
        public string DKTCH { get; set; }
        public int[] idxDKTCH = new int[] { 480, 1 };

        //Data consegna tassativa
        public string DKDCV { get; set; }
        public int[] idxDKDCV = new int[] { 481, 8 };

        //Tipo consegna -  Blank = prevista consegna, 1 = tassativa consegna, 2 = prenotazione, 3 = entro il, 4 = a partire dal, 5 = tassativa consegna + blocco spedizione
        public string DKT03 { get; set; }
        public int[] idxDKT03 = new int[] { 489, 1 };

        //Note consegna
        public string DKNOT { get; set; }
        public int[] idxDKNOT = new int[] { 490, 40 };

        //Ufficio Esecutivo
        public string DKT04 { get; set; }
        public int[] idxDKT04 = new int[] { 530, 1 };

        //Test libero 2
        public string DKT05 { get; set; }
        public int[] idxDKT05 = new int[] { 531, 1 };
        //Test libero 3
        public string DKT06 { get; set; }
        public int[] idxDKT06 = new int[] { 532, 1 };
        //Test libero 
        public string DKT07 { get; set; }
        public int[] idxDKT07 = new int[] { 533, 1 };
        //Test libero 5
        public string DKT08 { get; set; }
        public int[] idxDKT08 = new int[] { 534, 1 };

        //Anno borderò
        public string DKABO { get; set; }
        public int[] idxDKABO = new int[] { 535, 2 };

        //Filiale emissione bordero
        public string DKEBO { get; set; }
        public int[] idxDKEBO = new int[] { 537, 2 };

        //Key bordero
        public string DKKBO { get; set; }
        public int[] idxDKKBO = new int[] { 539, 7 };

        //Filiale partenza
        public string DKFPA { get; set; }
        public int[] idxDKFPA = new int[] { 546, 2 };

        //Zona
        public string DKZON { get; set; }
        public int[] idxDKZON = new int[] { 548, 5 };

        //Filiale competenza
        public string DKFIL { get; set; }
        public int[] idxDKFIL = new int[] { 553, 2 };

        //Soc competenza
        public string DKSOC { get; set; }
        public int[] idxDKSOC = new int[] { 555, 2 };

        //Imponibile valore merce - 3 decimali
        public string DKIF3 { get; set; }
        public int[] idxDKIF3 = new int[] { 557, 13 };

        //Divisa valore merce
        public string DKDI3 { get; set; }
        public int[] idxDKDI3 = new int[] { 570, 3 };








    }
}
