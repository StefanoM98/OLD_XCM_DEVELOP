using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNITEX_DOCUMENT_SERVICE.Model.Chiapparoli
{
    public class Chiapparoli_ShipmentIN
    {
        public string CodiceSocieta { get; set; }
        public int[] idxCodiceSocieta = new int[] { 0, 2 };

        public string SedeChiapparoli { get; set; }
        public int[] idxSedeChiapparoli = new int[] { 2, 2 };

        public string CodiceDitta { get; set; }
        public int[] idxCodiceDitta = new int[] { 4, 2 };

        public string NumeroProgressivoCHC { get; set; }
        public int[] idxNumeroProgressivoCHC = new int[] { 6, 9 };

        public string NumeroBordero { get; set; }
        public int[] idxNumeroBordero = new int[] { 15, 7 };

        public string DataBordero { get; set; }//AAMMGG
        public int[] idxDataBordero = new int[] { 22, 6 };

        public string OraBordero { get; set; }
        public int[] idxOraBordero = new int[] { 28, 4 };

        public string AnnoDDT { get; set; }
        public int[] idxAnnoDDT = new int[] { 32, 4 };

        public string NumeroDDT { get; set; }
        public int[] idxNumeroDDT = new int[] { 36, 7 };

        public string DataDDT { get; set; }//AAMMGG
        public int[] idxDataDDT = new int[] { 43, 6 };

        public string SerieBolla { get; set; }
        public int[] idxSerieBolla = new int[] { 49, 2 };

        public string Causale { get; set; }
        public int[] idxCausale = new int[] { 51, 2 };

        public string DescrizioneCausale { get; set; }
        public int[] idxDescrizioneCausale = new int[] { 53, 30 };

        public string NumDDTMandante { get; set; }
        public int[] idxNumDDTMandante = new int[] { 83, 7 };

        public string NumRifOrdine { get; set; }
        public int[] idxNumRifOrdine = new int[] { 90, 7 };

        public string RiferimentoOrdini { get; set; }
        public int[] idxRiferimentoOrdini = new[] { 97, 30 };

        public string DataOrdineCliente { get; set; }//AAMMGG
        public int[] idxDataOrdineCliente = new int[] { 127, 6 };

        public string Linea { get; set; }
        public int[] idxLinea = new int[] { 133, 2 };

        public string CodClienteIntestatario { get; set; }
        public int[] idxCodClienteIntestatario = new int[] { 135, 12 };

        public string CodClienteDestinatario { get; set; }
        public int[] idxCodClienteDestinatario = new int[] { 147, 12 };

        public string CodiceClienteDestinatarioCHC { get; set; }
        public int[] idxCodiceClienteDestinatarioCHC = new int[] { 159, 6 };

        public string RagSocialeDestinatario { get; set; }
        public int[] idxRagSocialeDestinatario = new int[] { 165, 30 };

        public string RagSocialeDestEXT { get; set; }
        public int[] idxRagSocialeDestEXT = new int[] { 195, 30 };

        public string IndirizzoDestinatario { get; set; }
        public int[] idxIndirizzoDestinatario = new int[] { 225, 30 };

        public string LocalitaDestinatario { get; set; }
        public int[] idxLocalitaDestinatario = new int[] { 255, 30 };

        public string CAPDestinatario { get; set; }
        public int[] idxCAPDestinatario = new int[] { 285, 5 };

        public string ProvDestinatario { get; set; }
        public int[] idxProvDestinatario = new int[] { 290, 2 };

        public string RegioneDestinatario { get; set; }
        public int[] idxRegioneDestinatario = new int[] { 292, 2 };

        public string NazioneDestinatario { get; set; }
        public int[] idxNazioneDestinatario = new int[] { 294, 3 };

        public string Inoltro { get; set; }//Y/N
        public int[] idxInoltro = new int[] { 297, 1 };

        public string CodiceVettore { get; set; }
        public int[] idxCodiceVettore = new int[] { 298, 3 };

        public string DescrizioneVettore { get; set; }
        public int[] idxDescrizioneVettore = new int[] { 301, 30 };

        public string Valuta { get; set; }
        public int[] idxValuta = new int[] { 331, 3 };

        public string ValoreOrdineContrassegno { get; set; }//2 dec
        public int[] idxValoreOrdineContrassegno = new int[] { 334, 11 };

        public string PortoFA { get; set; }//Franco - Assegnato
        public int[] idxPortoFA = new int[] { 345, 1 };

        public string NumeroRigheDDT { get; set; }
        public int[] idxNumeroRigheDDT = new int[] { 346, 4 };

        public string NumeroPezziDDT { get; set; }
        public int[] idxNumeroPezziDDT = new int[] { 350, 6 };

        public string NumeroColliDDT { get; set; }
        public int[] idxNumeroColliDDT = new int[] { 356, 3 };

        public string PesoKG { get; set; }
        public int[] idxPesoKG = new int[] { 359, 5 };

        public string VolumeM3 { get; set; }//3 dec
        public int[] idxVolumeM3 = new int[] { 364, 5 };

        public string DataConsegnaTassativa { get; set; } // formato??
        public int[] idxDataConsegnaTassativa = new int[] { 369, 6 };

        public string SegnacolloIniziale { get; set; }
        public int[] idxSegnacolloIniziale = new int[] { 375, 7 };

        public string SegnacolloFinale { get; set; }
        public int[] idxSegnacolloFinale = new int[] { 382, 7 };

        public string ValutaValCostoSpedizione { get; set; }
        public int[] idxValutaValCostoSpedizione = new int[] { 389, 3 };

        public string CostoSpedizione { get; set; }//autofattura - 2 dec
        public int[] idxCostoSpedizione = new int[] { 392, 11 };

        public string ValoreSpedizioneCorriere { get; set; }//2 dec
        public int[] idxValoreSpedizioneCorriere = new int[] { 403, 11 };

        public string SimulazioneValoreSpedizione { get; set; }//2 dec
        public int[] idxSimulazioneValoreSpedizione = new int[] { 414, 11 };

        public string DataConsegnaCliente { get; set; }
        public int[] idxDataConsegnaCliente = new int[] { 425, 6 };

        public string DefinizioneIMS { get; set; }
        public int[] idxDefinizioneIMS = new int[] { 431, 3 };

        public string CampoTest1 { get; set; }
        public int[] idxCampoTest1 = new int[] { 434, 1 };

        public string CampoTest2 { get; set; }
        public int[] idxCampoTest2 = new int[] { 435, 1 };

        public string CampoTest3 { get; set; }
        public int[] idxCampoTest3 = new int[] { 436, 1 };

        public string CampoTest4 { get; set; }
        public int[] idxCampoTest4 = new int[] { 437, 1 };

        public string CampoTest5 { get; set; }
        public int[] idxCampoTest5 = new int[] { 438, 1 };

        public string CampoTest6 { get; set; }
        public int[] idxCampoTest6 = new int[] { 439, 1 };

        public string NoteConsegna { get; set; }
        public int[] idxNoteConsegna = new int[] { 440, 40 };

        public List<string> Vincoli = new List<string>();
    }
    public class Chiapparoli_DettaglioColli
    {
        public string CodiceSocieta { get; set; }
        public int[] idxCodiceSocieta = new int[] { 0, 2 };

        public string CodiceMagazzino { get; set; }//ha stesso riferimento di CodiceSocieta?!?!?!?!?!!?
        public int[] idxCodiceMagazzino = new int[] { 0, 2 };

        public string CodiceDitta { get; set; }
        public int[] idxCodiceDitta = new int[] { 2, 2 };

        public string NumeroProgressivoCHC { get; set; }
        public int[] idxNumeroProgressivoCHC = new int[] { 6, 9 };

        public string IDCollo { get; set; }
        public int[] idxIDCollo = new int[] { 15, 15 };

        public string IDBancale { get; set; }
        public int[] idxIDBancale = new int[] { 30, 15 };
    }
    public class Chiapparoli_DatiAccessori
    {
        public string CodiceMagazzino { get; set; }
        public int[] idxCodiceMagazzino = new int[] { 0, 2 };

        public string CodiceDitta { get; set; }
        public int[] idxCodiceDitta = new int[] { 2, 2 };

        public string NumeroProgressivoCHC { get; set; }
        public int[] idxNumeroProgressivoCHC = new int[] { 6, 9 };

        public string NumeroBordero { get; set; }
        public int[] idxNumeroBordero = new int[] { 15, 7 };

        public string DataBordero { get; set; }//AAMMGG
        public int[] idxDataBordero = new int[] { 22, 6 };

        public string OraBordero { get; set; }//HHMM
        public int[] idxOraBordero = new int[] { 28, 4 };

        public string IDServizioAggiuntivo { get; set; }
        public int[] idxIDServizioAggiuntivo = new int[] { 32, 15 };

        public string Descrizione { get; set; }
        public int[] idxDescrizione = new int[] { 47, 25 };

        //public string NoteRigaServizio { get; set; }
        //public int[] idxNoteRigaServizio = new int[] { 70, 70 };

        //public string CostoConcordato { get; set; }//2 dec
        //public int[] idxCostoConcordato = new int[] { 140, 9 };
    }
    public class Chiapparoli_Esiti
    {
        public string SedeChiapparoli { get; set; }
        public int[] idxSedeChiapparoli = new int[] { 0, 2 };

        public string CodiceDitta { get; set; }
        public int[] idxCodiceDitta = new int[] { 2, 2 };

        public string NumeroProgressivo { get; set; }//CHC???
        public int[] idxNumeroProgressivo = new int[] { 4, 9 };

        public string PosizioneRiga { get; set; }
        public int[] idxPosizioneRiga = new int[] { 13, 3 };

        public string CodiceResa { get; set; }
        public int[] idxCodiceResa = new int[] { 16, 4 };

        public string DataResa { get; set; }//AAMMGG
        public int[] idxDataResa = new int[] { 20, 6 };

        public string OraResa { get; set; }//HHMMSS
        public int[] idxOraResa = new int[] { 26, 6 };

        public string RigaNote { get; set; }
        public int[] idxRigaNote = new int[] { 32, 40 };

        public string RiferimentoVettore { get; set; }
        public int[] idxRiferimentoVettore = new int[] { 72, 15 };

        public string DataRiferimentoVettore { get; set; }//AAMMGG
        public int[] idxDataRiferimentoVettore = new int[] { 87, 6 };

        public string RiferimentoSubVettore { get; set; }
        public int[] idxRiferimentoSubVettore = new int[] { 93, 15 };

        public string DataRiferimentoSubVettore { get; set; }//AAMMGG
        public int[] idxDataRiferimentoSubVettore = new int[] { 108, 6 };

        public string Colli { get; set; }
        public int[] idxColli = new int[] { 114, 6 };

        public string PesoKG { get; set; }//2 dec
        public int[] idxPesoKG = new int[] { 120, 8 };

        public string Volume { get; set; }//3 dec
        public int[] idxVolume = new int[] { 128, 8 };

        public string PesoTassato { get; set; }//nessun decimale??
        public int[] idxPesoTassato = new int[] { 128, 8 };

        public string ImportoTotaleSpedizione { get; set; }//2 dec
        public int[] idxImportoTotaleSpedizione = new int[] { 142, 9 };

        public string Filler { get; set; }
        public int[] idxFiller = new int[] { 151, 9 };

    }
    public class Chiapparoli_FatturaCorriere
    {
        public string CodiceVettore { get; set; }
        public int[] idxCodiceVettore = new int[] { 0, 3 };

        public string CodiceSocieta { get; set; }
        public int[] idxCodiceSocieta = new int[] { 3, 2 };

        public string SedeChiapparoli { get; set; }
        public int[] idxSedeChiapparoli = new int[] { 5, 2 };

        public string CodiceDitta { get; set; }
        public int[] idxCodiceDitta = new int[] { 7, 2 };

        public string NumeroProgressivoCHC { get; set; }
        public int[] idxNumeroProgressivoCHC = new int[] { 9, 9 };

        public string AnnoDDT { get; set; }
        public int[] idxAnnoDDT = new int[] { 18, 4 };

        public string NumeroDDT { get; set; }
        public int[] idxNumeroDDT = new int[] { 22, 7 };

        public string NumeroColliDDT { get; set; }
        public int[] idxNumeroColliDDT = new int[] { 29, 6 };

        public string PesoKG { get; set; }//2 dec
        public int[] idxPesoKG = new int[] { 35, 8 };

        public string Volume { get; set; }//3 dec
        public int[] idxVolume = new int[] { 43, 8 };

        public string PesoTassato { get; set; }
        public int[] idxPesoTassato = new int[] { 51, 6 };

        public string Voce { get; set; }
        public int[] idxVoce = new int[] { 57, 15 };

        public string Importo { get; set; }//2 dec
        public int[] idxImporto = new int[] { 72, 9 };

        public string NumeroFatturaCorriere { get; set; }
        public int[] idxNumeroFatturaCorriere = new int[] { 81, 12 };

        public string DataFatturaCorriere { get; set; }
        public int[] idxDataFatturaCorriere = new int[] { 93, 6 };

        public string RiferimentoVettore { get; set; }
        public int[] idxRiferimentoVettore = new int[] { 99, 15 };

        public string ValoreVolume { get; set; }
        public int[] idxValoreVolume = new int[] { 114, 9 };

        public string Note { get; set; }
        public int[] idxNote = new int[] { 123, 100 };
    }
}
