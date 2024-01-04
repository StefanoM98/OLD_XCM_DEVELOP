using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNITEX_DOCUMENT_SERVICE.Model.StockHouse
{
    public class StockHouse_Esiti_OUT
    {
        //Codice univoco mandante, per distinguere bolle di case mandanti diverse
        public string MANDANTE { get; set; }
        public int[] idxMANDANTE = new int[] { 0, 15 };

        //Codice univoco, per un proprietarion, della bolla di spedizione.
        public string NUMERO_BOLLA { get; set; }
        public int[] idxNUMERO_BOLLA = new int[] { 15, 15 };

        //Data spedizione yyyyMMdd
        public string DATA_BOLLA { get; set; }
        public int[] idxDATA_BOLLA = new int[] { 30, 8 };

        //Ragione sociale vettore
        public string RAGIONE_SOCIALE_VETTORE { get; set; }
        public int[] idxRAGIONE_SOCIALE_VETTORE = new int[] { 38, 30 };

        //Data in cui il corriere prende la merce in carico dal deposito StockHouse
        public string DATA_PRESA_CONS { get; set; }
        public int[] idxDATA_PRESA_CONS = new int[] { 68, 8 };

        //CON - Consegnata / PRE - Riprogrammata, preso appuntamento.
        public string STATO_CONSEGNA { get; set; }
        public int[] idxSTATO_CONSEGNA = new int[] { 76, 3 };

        //Descrizione stato consegna
        public string DESCRIZIONE_STATO_CONSEGNA { get; set; }
        public int[] idxDESCRIZIONE_STATO_CONSEGNA = new int[] { 79, 30 };

        //Data consegna o appuntamento yyyyMMdd
        public string DATA { get; set; }
        public int[] idxDATA = new int[] { 109, 8 };

    }
}
