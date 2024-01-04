using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNITEX_DOCUMENT_SERVICE.Model.StockHouse
{
    public class StockHouse_Shipment_IN
    {
        ///MANDANTE
        public string MANDANTE { get; set; }
        public int[] idxMANDANTE = new int[] { 0, 15 };

        ///NR_BOLLA
        public string NR_BOLLA { get; set; }
        public int[] idxNRBOLLA = new int[] { 15, 15 };

        ///DATA_BOLLA
        public string DATA_BOLLA { get; set; }
        public int[] idxDATA_BOLLA = new int[] { 30, 8 };

        ///NR_SHIPMENT
        public string NR_SHIPMENT { get; set; }
        public int[] idxNR_SHIPMENT = new int[] { 38, 15 };

        ///RAG_SOC_MITTENTE
        public string RAG_SOC_MITTENTE { get; set; }
        public int[] idxRAG_SOC_MITTENTE = new int[] { 53, 30 };

        ///INDIRIZZO_MITTENTE
        public string INDIRIZZO_MITTENTE { get; set; }
        public int[] idxINDIRIZZO_MITTENTE = new int[] { 83, 30 };

        //CAP_MITTENTE
        public string CAP_MITTENTE { get; set; }
        public int[] idxCAP_MITTENTE = new int[] { 113, 5 };

        //LOC_MITTENTE
        public string LOC_MITTENTE { get; set; }
        public int[] idxLOC_MITTENTE = new int[] { 118, 30 };

        //PROV_MITTENTE
        public string PROV_MITTENTE { get; set; }
        public int[] idxPROV_MITTENTE = new int[] { 148, 4 };

        //NAZIONE_MITTENTE
        public string NAZIONE_MITTENTE { get; set; }
        public int[] idxNAZIONE_MITTENTE = new int[] { 152, 30 };

        //RAG_SOC_DESTINATARIO
        public string RAG_SOC_DESTINATARIO { get; set; }
        public int[] idxRAG_SOC_DESTINATARIO = new int[] { 182, 30 };

        //INDIRIZZO_DESTINATARIO
        public string INDIRIZZO_DESTINATARIO { get; set; }
        public int[] idxINDIRIZZO_DESTINATARIO = new int[] { 212, 30 };

        //CAP_DESTINATARIO
        public string CAP_DESTINATARIO { get; set; }
        public int[] idxCAP_DESTINATARIO = new int[] { 242, 5 };

        //LOC_DESTINATARIO
        public string LOC_DESTINATARIO { get; set; }
        public int[] idxLOC_DESTINATARIO = new int[] { 247, 30 };

        //PROV_DESTINATARIO
        public string PROV_DESTINATARIO { get; set; }
        public int[] idxPROV_DESTINATARIO = new int[] { 277, 4 };

        //NAZIONE_DESTINATARIO
        public string NAZIONE_DESTINATARIO { get; set; }
        public int[] idxNAZIONE_DESTINATARIO = new int[] { 281, 30 };

        //PESO_SPEDIZIONE
        public string PESO_SPEDIZIONE { get; set; }
        public int[] idxPESO_SPEDIZIONE = new int[] { 311, 15 };

        //VOLUME_SPEDIZIONE
        public string VOLUME_SPEDIZIONE { get; set; }
        public int[] idxVOLUME_SPEDIZIONE = new int[] { 326, 15 };

        //Numero Cartoni CT
        public string N_CARTONI_CT { get; set; }
        public int[] idxN_CARTONI_CT = new int[] { 341, 3 };

        //Numero Bancali BA
        public string N_BANCALI_BA { get; set; }
        public int[] idxN_BANCALI_BA = new int[] { 344, 3 };

        //Numero Bancali Collettame BB
        public string N_BANCALI_COLLETTAME_BB { get; set; }
        public int[] idxN_BANCALI_COLLETTAME_BB = new int[] { 347, 3 };

        //Numero Bancali + Bancali Collettame
        public string N_BA_BB { get; set; }
        public int[] idxN_BA_BB = new int[] { 350, 3 };

        //Peso Cartoni
        public string PESO_CARTONI_CT { get; set; }
        public int[] idxPESO_CARTONI_CT = new int[] { 353, 9 };

        //Valuta Contrassegno
        public string VALUTA_CONTRASS { get; set; }
        public int[] idxVALUTA_CONTRASS = new int[] { 362, 9 };

        //IMPORTO_CONTRASS
        public string IMPORTO_CONTRASS { get; set; }
        public int[] idxIMPORTO_CONTRASS = new int[] { 371, 18 };

        //NUMERO_COLLI_SPED
        public string NUMERO_COLLI_SPED { get; set; }
        public int[] idxNUMERO_COLLI_SPED = new int[] { 389, 9 };

        //DA_SEGNACOLLO
        public string DA_SEGNACOLLO { get; set; }
        public int[] idxDA_SEGNACOLLO = new int[] { 398, 9 };

        //A_SEGNACOLLO
        public string A_SEGNACOLLO { get; set; }
        public int[] idxA_SEGNACOLLO = new int[] { 407, 9 };

        //NOTE
        public string NOTE { get; set; }
        public int[] idxNOTE = new int[] { 416, 80 };

        //VETTORE
        public string VETTORE { get; set; }
        public int[] idxVETTORE = new int[] { 496, 6 };

        //NR_DISTINTA
        public string NR_DISTINTA { get; set; }
        public int[] idxNR_DISTINTA = new int[] { 502, 10 };

        //DT_DISTINTA
        public string DT_DISTINTA { get; set; }
        public int[] idxDT_DISTINTA = new int[] { 512, 8 };

        //COND_PAG
        public string COND_PAG { get; set; }
        public int[] idxCOND_PAG = new int[] { 520, 3 };

        //CONS_PIANI
        public string CONS_PIANI { get; set; }
        public int[] idxCONS_PIANI = new int[] { 523, 1 };

        //TEL_PRIMA_CONS
        public string TEL_PRIMA_CONS { get; set; }
        public int[] idxTEL_PRIMA_CONS = new int[] { 524, 20 };

        //DT_CONS_TASSAT_1
        public string DT_CONS_TASSAT_1 { get; set; }
        public int[] idxDT_CONS_TASSAT_1 = new int[] { 544, 8 };

        //DT_CONS_TASSAT_2
        public string DT_CONS_TASSAT_2 { get; set; }
        public int[] idxDT_CONS_TASSAT_2 = new int[] { 552, 8 };

        //NOTE_1
        public string NOTE_1 { get; set; }
        public int[] idxNOTE_1 = new int[] { 560, 65 };

        //NOTE_2
        public string NOTE_2 { get; set; }
        public int[] idxNOTE_2 = new int[] { 625, 65 };

        //NOTE_3
        public string NOTE_3 { get; set; }
        public int[] idxNOTE_3 = new int[] { 690, 65 };

        //NOTE_4
        public string NOTE_4 { get; set; }
        public int[] idxNOTE_4 = new int[] { 755, 65 };

        //NOTE_5
        public string NOTE_5 { get; set; }
        public int[] idxNOTE_5 = new int[] { 820, 50 };

        //LIBERO
        public string Libero { get; set; }
        public int[] idxLibero = new int[] { 870, 3 };

        //LIBERO
        public string Libero_1 { get; set; }
        public int[] idxLibero_1 = new int[] { 873, 3 };

        //N_PALLETTS
        public string N_PALLETTS { get; set; }
        public int[] idxN_PALLETTS = new int[] { 876, 3 };

        //N_CHEP
        public string N_CHEP { get; set; }
        public int[] idxN_CHEP = new int[] { 879, 3 };

        //N_EPAL
        public string N_EPAL { get; set; }
        public int[] idxN_EPAL = new int[] { 882, 3 };


        //A = ANN
        public string AANN { get; set; }
        public int[] idxAANN = new int[] { 885, 1 };


        //T = TRAS
        public string TTRAS { get; set; }
        public int[] idxTTRAS = new int[] { 886, 1 };

        //M = MAN, A = AUT
        public string M_A { get; set; }
        public int[] idxM_A = new int[] { 887, 1 };

        //NR_PREBOLLA
        public string NR_PREBOLLA { get; set; }
        public int[] idxNR_PREBOLLA = new int[] { 888, 10 };


        //= Y /Blank
        public string LEAD_TIME { get; set; }
        public int[] idxLEAD_TIME = new int[] { 898, 1 };

        //Libero_2
        public string Libero_2 { get; set; }
        public int[] idxLibero_2 = new int[] { 899, 40 };


    }
}
