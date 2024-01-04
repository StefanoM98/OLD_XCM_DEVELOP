using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNITEX_DOCUMENT_SERVICE.Model.CDL
{
    public class CDL_ShipmentOUT
    {
        /// <summary>
        /// I CAMPI DATA E NUMRICI SONO EVENTUALMENTE RIEMPITI CON 0 (ZERI) E ALLINEATI A DESTRA
        ///I CAMPI STRINGA SONO EVENTUALMENTE RIEMPITI CON ' ' (LO SPAZIO) E SONO ALLINEATI A SINISTR
        /// </summary>
        private static int CalcolaLunghezza(int v, int v1)
        {
            return v1 - v;
        }
        public string AnnoFiliale { get; set; }//1 6 NUMERICO YYYY01 (01 CAMPO FISSO)
        public int[] idxAnnoFiliale = new int[] { 0, CalcolaLunghezza(1,6) };       
        public string RiferimentoSpedizione { get; set; }//7 14 NUMERICO Nostro riferimento univoco
        public int[] idxRiferimentoSpedizione = new int[] { 6, CalcolaLunghezza(6, 14) };
        public string DataSpedizione { get; set; }//19 26 DATA FORMATO:YYYYMMDD
        public int[] idxDataSpedizione = new int[] { 18, CalcolaLunghezza(18, 26) };
        public string Mittente { get; set; }//53 82 STRINGA
        public int[] idxMittente = new int[] { 52, CalcolaLunghezza(52, 82) };
        public string IndirizzoMittente { get; set; }//83 112 STRINGA
        public int[] idxIndirizzoMittente = new int[] { 82, CalcolaLunghezza(82, 112) };
        public string LocalitaMittente { get; set; }//113 142 STRINGA
        public int[] idxLocalitaMittente = new int[] { 112, CalcolaLunghezza(112, 142) };
        public string ProvinciaMittente { get; set; }//143 147 STRINGA
        public int[] idxProvinciaMittente = new int[] { 142, CalcolaLunghezza(142, 147) };
        public string CapMittente { get; set; }//153 162 STRINGA
        public int[] idxCapMittente = new int[] { 152, CalcolaLunghezza(152, 162) };
        public string NumeroDdtMittenteOriginale { get; set; }//163 177 STRINGA Rif. mittente sul collo
        public int[] idxNumeroDdtMittenteOriginale = new int[] { 162, CalcolaLunghezza(162, 177) };
        public string AltroRiferimento { get; set; }//201 220 STRINGA Altro rif. mittente sul collo
        public int[] idxAltroRiferimento = new int[] { 200, CalcolaLunghezza(200, 220) };
        public string SEC_Riferimento { get; set; }// 221 240 STRINGA
        public int[] idxSEC_Riferimento = new int[] { 220, CalcolaLunghezza(220, 240) };
        public string RagSocDestinatario { get; set; }//243 272 STRINGA
        public int[] idxRagSocDestinatario = new int[] { 242, CalcolaLunghezza(242, 272) };
        public string IndirizzoDestinatario { get; set; }//273 302 STRINGA
        public int[] idxIndirizzoDestinatario = new int[] { 272, CalcolaLunghezza(272, 302) };
        public string LocalitaDestinatario { get; set; }//303 332 STRINGA
        public int[] idxLocalitaDestinatario = new int[] { 302, CalcolaLunghezza(302, 332) };
        public string ProvinciaDestinatario { get; set; }// 333 337 STRINGA
        public int[] idxProvinciaDestinatario = new int[] { 332, CalcolaLunghezza(332, 337) };
        public string CapDestinatario { get; set; }//343 352 STRINGA
        public int[] idxCapDestinatario = new int[] { 342, CalcolaLunghezza(342, 352) };
        public string MittenteOriginale { get; set; }//353 367 STRINGA Nominativo della mittente
        public int[] idxMittenteOriginale = new int[] { 352, CalcolaLunghezza(352, 367) };
        public string Colli { get; set; }//373 377 NUMERICO
        public int[] idxColli = new int[] { 372, CalcolaLunghezza(372, 377) };
        public string Peso { get; set; }//378 385 NUMERICO CON 3 DECIMALI
        public int[] idxPeso = new int[] { 377, CalcolaLunghezza(377, 385) };
        public string Volume { get; set; }//386 393 NUMERICO CON 3 DECIMALI
        public int[] idxVolume = new int[] { 385, CalcolaLunghezza(385, 393) };
        public string Bancali { get; set; }//394 396 NUMERICO
        public int[] idxBancali = new int[] { 393, CalcolaLunghezza(393, 396) };
        public string EPAL { get; set; }//397 399 NUMERICO
        public int[] idxEPAL = new int[] { 396, CalcolaLunghezza(396, 399) };
        public string MetriLineari { get; set; }//400 406 NUMERICO CON 2 DECIMALI
        public int[] idxMetriLineari = new int[] { 399, CalcolaLunghezza(399, 406) };
        public string TipoIncassoRichiesto { get; set; }//412 412 STRINGA Eventuale tipo Incasso Assegno: A = Assegno Intestato, C TIPO INCASSO RICHIESTO 412 412 STRINGA = Contanti, D = Assegno, B = Bonifico
        public int[] idxTipoIncassoRichiesto = new int[] { 411, CalcolaLunghezza(411, 412) };
        public string Contrassegno { get; set; }//413 425 NUMERICO CON 4 DECIMALI
        public int[] idxContrassegno = new int[] { 412, CalcolaLunghezza(412, 425) };
        public string Anticipate { get; set; }//429 441 NUMERICO CON 4 DECIMALI
        public int[] idxAnticipate = new int[] { 428, CalcolaLunghezza(428, 441) };
        public string PrimoSegnacollo { get; set; }//443 450 NUMERICO
        public int[] idxPrimoSegnacollo = new int[] { 442, CalcolaLunghezza(442, 450) };
        public string TipoSegnacollo { get; set; }//451 452 STRINGA Contiene: X o A (vuol dire AL)
        public int[] idxTipoSegnacollo = new int[] { 450, CalcolaLunghezza(450, 452) };
        public string UltimoSegnacollo { get; set; }// 453 460 NUMERICO
        public int[] idxUltimoSegnacollo = new int[] { 452, CalcolaLunghezza(452, 460) };
        public string TipoDataConsegnaTassativa { get; set; }//516 516 STRINGA Vuota = Tassativa il, E = Entro Il, D = Dopo Il
        public int[] idxTipoDataConsegnaTassativa = new int[] { 515, CalcolaLunghezza(515, 516) };
        public string DataConsegnaTassativa { get; set; }//517 524 DATA FORMATO:YYYYMMDD
        public int[] idxDataConsegnaTassativa = new int[] { 516, CalcolaLunghezza(516, 524) };
        public string DataAppuntamento { get; set; }//525 532 DATA FORMATO:YYYYMMDD
        public int[] idxDataAppuntamento = new int[] { 524, CalcolaLunghezza(524, 532) };
        public string NumeroBordero { get; set; }// 533 538 NUMERICO
        public int[] idxNumeroBordero = new int[] { 532, CalcolaLunghezza(532, 538) };
        public string DataBordero { get; set; }//539 546 DATA FORMATO:YYYYMMDD
        public int[] idxDataBordero = new int[] { 538, CalcolaLunghezza(538, 546) };
        public string NoteSpedizioni { get; set; }//550 749 STRINGA
        public int[] idxNoteSpedizioni = new int[] { 549, CalcolaLunghezza(549, 749) };

        public override string ToString()
        {
            return $"{AnnoFiliale}{RiferimentoSpedizione}{" 1  "}{DataSpedizione}{"   "}{"        "}{"  "}{"             "}{Mittente}{IndirizzoMittente}{LocalitaMittente}{ProvinciaMittente}{CapMittente}{NumeroDdtMittenteOriginale}{"        "}{AltroRiferimento}{"                      "}{SEC_Riferimento}{RagSocDestinatario}{IndirizzoDestinatario}{LocalitaDestinatario}{ProvinciaDestinatario}{"     "}{CapDestinatario}{MittenteOriginale}{Colli}{Peso}{Volume}{Bancali}{EPAL}{MetriLineari}{"     "}{TipoIncassoRichiesto}{Contrassegno}{"   "}{Anticipate}{" "}{PrimoSegnacollo}{TipoSegnacollo}{UltimoSegnacollo}{"     N                   0000000000000E0000000     0000 "}{DataConsegnaTassativa}{DataAppuntamento}{NumeroBordero}{DataBordero}{NoteSpedizioni}";
        }
    }

    public class CDL_ShipmentOUTColli
    {
        public string BarcodeBolla { get; set; }//da 1 a 14 RIFERIMENTO UNIVOCO (SPED. CDL DAL 7 AL 1 RIFERIMENTO UNIVOCO (SPED. CDL DAL 7 AL 14)
        public string Vuoto1C { get
            {
                return " ";
            }
        }
        public string Esercizio { get; set; }//da 16 a 19 - Numerico - ANNO
        public string IDCliente { get; set; }//da 20 a 27 - Numerico - FORNITO DAL CLIENTE DI CONSULT
        public string NumeroColli { get; set; }//da 28 a 32 - Numerico 
        public string Bancali { get; set; }//da 33 a 37 - Numerico - 
        public string PrimoSegnacollo { get; set; }//da 38 a 45 - Numerico 
        public string TipoSegnacollo { get; set; }//da 46 a 47 - Stringa
        public string UltimoSegnacollo { get; set; }//da 48 55 - Numerico
        public string BarcodeSegnacollo { get; set; }//da 56 a 85 - Stringa

        public override string ToString()
        {
            return $"{BarcodeBolla}{Vuoto1C}{Esercizio}{IDCliente}{NumeroColli}{Bancali}{PrimoSegnacollo}{TipoSegnacollo}{Vuoto1C}{UltimoSegnacollo}{BarcodeSegnacollo}";
        }
    }
}
