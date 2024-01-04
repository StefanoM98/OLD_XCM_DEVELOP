using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNITEX_DOCUMENT_SERVICE.Model._3C
{
    public class _3C_ShipmentIN
    {
        /// <summary>
        /// SE TIPO TRASPORTO = 'B' SARà UN RECORD RELATIVO AL DETTAGLIO BARCODE COLLI. IN QUESTO CASO I CAMPI COLLI
        /// PESO E VOLUME SARANNO A ZERO E SARA' COMPILATO IL CAMPO BARCODE SEGNACOLLO
        /// ****  E' il riferimento da utilizzare per la restituzione degli esiti di consegna.
        /// SE TIPO TRASPORTO = 'D' SARà UN RECORD RELATIVO AL DETTAGLIO MISURE COLLO/PALLET.
        /// IN QUESTO CASO I CAMPI SIGNIFICATIVI, OLTRE AL NR. SPEDIZIONE, SARANNO:
        /// 
        /// SE TIPO TRASPORTO = 'D' SARà UN RECORD RELATIVO AL DETTAGLIO MISURE COLLO/PALLET.					
        ///IN QUESTO CASO I CAMPI SIGNIFICATIVI, OLTRE AL NR. SPEDIZIONE, SARANNO:					
        ///COLLI		145	149		NR. COLLI/PALLET
        ///PESO		150	156		PESO UNITARIO RIFERITO A QUANTO INDICATO NEL CAMPO COLLI
        ///annotazioni		321	380		LARGHEZZA IN CM DA POS. 321 A POS 323
        ///annotazioni		381	440		LUNGHEZZA IN CM DA POS. 381 A POS 383
        ///annotazioni		441	500		ALTEZZA IN CM DA POS. 441 A POS. 443 
        /// </summary> ma stiamo scherzando?!?!?
        public string TipoTrasporto { get; set; }//tipo trasporto F = porto Franco, A = porto Assegnato
        public int[] idxTipoTrasporto = new int[] { 0, 1 };

        public string NumeroBolla { get; set; }
        public int[] idxNumeroBolla = new int[] { 1, 15 };

        public string DataBolla { get; set; }
        public int[] idxDataBolla = new int[] { 16, 8 };

        public string RagioneSocialeDestinatario { get; set; }
        public int[] idxRagioneSocialeDestinatario = new int[] { 24, 40 };

        public string IndirizzoDestinatario { get; set; }
        public int[] idxIndirizzoDestinatario = new int[] { 64, 40 };

        public string LocalitaDestinatario { get; set; }
        public int[] idxLocalitaDestinatario = new int[] { 104, 30 };

        public string CAPDestinatario { get; set; }
        public int[] idxCAPDestinatario = new int[] { 134, 5 };

        public string ProvDestinatario { get; set; }
        public int[] idxProvDestinatario = new int[] { 139, 2 };

        public string ZonaDiConsegna { get; set; }
        public int[] idxZonaDiConsegna = new int[] { 141, 3 };

        public string Colli { get; set; }
        public int[] idxColli = new int[] { 144, 5 };

        public string Peso1D { get; set; }//1dec
        public int[] idxPeso1D = new int[] { 149, 7 };

        public string Volume2D { get; set; }//2dec
        public int[] idxVolume2D = new int[] { 156, 5 };

        public string NumeroSegnacolliStampati { get; set; }
        public int[] idxNumeroSegnacolliStampati = new int[] { 161, 3 };

        public string DataConsegnaTassativa { get; set; }//formato? SAMG??
        public int[] idxDataConsegnaTassativa = new int[] { 164, 8 };

        public string TipoConsegnaTassativa { get; set; }//Tipo consegna tassativa -->  T = Tassativa x il,  E = tassativa Entro il, P = Prenotata x il.
        public int[] idxTipoConsegnaTassativa = new int[] { 172, 1 };

        public string Contrassegno2D { get; set; }//2dec
        public int[] idxContrassegno2D = new int[] { 173, 11 };

        public string TotaleDaIncassare2D { get; set; }//2dec
        public int[] idxTotaleDaIncassare2D = new int[] { 184, 11 };

        public string RagioneSocialeMittenteOriginale { get; set; }
        public int[] idxRagioneSocialeMittenteOriginale = new int[] { 195, 40 };

        public string IndirizzoMittenteOriginale { get; set; }
        public int[] idxIndirizzoMittenteOriginale = new int[] { 235, 40 };

        public string LocalitaMittenteOriginale { get; set; }
        public int[] idxLocalitaMittenteOriginale = new int[] { 275, 30 };

        public string RiferimentoMittenteOriginale { get; set; }
        public int[] idxRiferimentoMittenteOriginale = new int[] { 312, 8 };

        public string Annotazioni1 { get; set; }
        public int[] idxAnnotazioni1 = new int[] { 320, 60 };

        public string Annotazioni2 { get; set; }
        public int[] idxAnnotazioni2 = new int[] { 380, 60 };

        public string Annotazioni3 { get; set; }
        public int[] idxAnnotazioni3 = new int[] { 440, 60 };

        public string NumeroTelefonoPreavviso { get; set; }
        public int[] idxNumeroTelefonoPreavviso = new int[] { 500, 15 };

        public string GiorniChiusura { get; set; }//giorni di chiusura = ognuno dei 7 caratteri corrisponde ad un giorno della settimana 1= Lunedì, 7 = Domenica - M = chiuso mattino, P = chiuso Pomeriggio, T = chiuso Tutto il giorno
        public int[] idxGiorniChiusura = new int[] { 515, 7 };

        public string PrimoSegnacollo { get; set; }
        public int[] idxPrimoSegnacollo = new int[] { 522, 6 };

        public string UltimoSegnacollo { get; set; }
        public int[] idxUltimoSegnacollo = new int[] { 528, 6 };

        public string ValoreMerce2D { get; set; }//2dec
        public int[] idxValoreMerce2D = new int[] { 534, 11 };

        public string IDBarcodeSegnacollo { get; set; }
        public int[] idxIDBarcodeSegnacollo = new int[] { 545, 15 };

        public string NumeroBorderau { get; set; }
        public int[] idxNumeroBorderau = new int[] { 560, 13 };

        public string BarcodeSegnacollo { get; set; }
        public int[] idxBarcodeSegnacollo = new int[] { 573, 30 };

        public string Filler { get; set; }
        public int[] idxFiller = new int[] { 603, 16 };

        public string FineRecord { get { return "."; } }

        List<_3C_ShipmentINColli> segnacolliDellaSpedizione = new List<_3C_ShipmentINColli>();
    }
    public class _3C_ShipmentINColli
    {
        public string segnacollo { get; set; }
    }
}
