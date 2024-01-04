using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNITEX_DOCUMENT_SERVICE.Model.Chiapparoli
{
    public class Chiapparoli_EsitiOUT
    {
        public string SedeChiapparoli { get; set; }
        public int[] idxSedeChiapparoli = new int[] { 0, 2 };
        public string CodiceDitta { get; set; }
        public int[] idxCodiceDitta = new int[] { 2, 2 };
        public string NumeroProgressivo { get; set; }//CHC??
        public int[] idxNumeroProgressivo = new int[] { 4, 9 };
        public string PosizioneRiga { get; set; }// che significa?!?!?
        public int[] idxPosizioneRiga = new int[] { 13, 3 };
        public string CodiceResa { get; set; }
        public int[] idxCodiceResa = new int[] { 16, 4 };
        public string DataResaAAMMGG { get; set; }//AAMMGG
        public int[] idxDataResaAAMMGG = new int[] { 20, 6 };
        public string OraResa { get; set; }
        public int[] idxOraResa = new int[] { 26, 6 };
        public string RigaNote { get; set; }
        public int[] idxRigaNote = new int[] { 32, 40 };
        public string RiferimentoVettore { get; set; }
        public int[] idxRiferimentoVettore = new int[] { 72, 15 };
        public string DataRiferimentoVettoreAAMMGG { get; set; }
        public int[] idxDataRiferimentoVettore = new int[] { 87, 6 };
        public string RiferimentoSubVettore { get; set; }
        public int[] idxRiferimentoSubVettore = new int[] { 93, 15 };
        public string DataRiferimentoSubVettoreAAMMGG { get; set; }
        public int[] idxDataRiferimentoSubVettore = new int[] { 108, 6 };
        public string Colli { get; set; }
        public int[] idxColli = new int[] { 114, 6 };
        public string Peso2d { get; set; }//2dec
        public int[] idxPeso2d = new int[] { 120, 8 };
        public string Volume3d { get; set; }//3dec
        public int[] idxVolume3d = new int[] { 128, 8 };
        public string PesoTassato { get; set; }
        public int[] idxPesoTassato = new int[] { 136, 6 };
        public string ImportoTotaleSpedizione2d { get; set; }//2dec
        public int[] idxImportoTotaleSpedizione = new int[] { 142, 9 };
        public string Filler { get; set; }
        public int[] idxFiller = new int[] { 0, 0 };

        public int statoUNITEX { get; set; }
    }

    public class Chiapparoli_StatiDocumento
    {
        public int IdUnitex { get; set; }
        public string CodiceStato { get; set; }
        public string DescrizioneStato { get; set; }
        public static Chiapparoli_StatiDocumento FromCsv(string csvLine)
        {
            var values = csvLine.Split(';');
            Chiapparoli_StatiDocumento stato = new Chiapparoli_StatiDocumento();
            stato.IdUnitex = Convert.ToInt32(values[0]);
            stato.CodiceStato = values[1];
            stato.DescrizioneStato = values[2];
            return stato;

        }
    }
}
