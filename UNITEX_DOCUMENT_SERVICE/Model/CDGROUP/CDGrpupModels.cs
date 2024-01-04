using System;
using System.Collections.Generic;

public class CDGROUP_EsitiOUT
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

    //Località di scarico
    public string LOCALITA { get; set; }
    public int[] idxLOCALITA = new int[] { 117, 15 };

    //Riferimento Unitex
    public string RIFVETTORE { get; set; }
    public int[] idxRIFVETTORE = new int[] { 132, 15 };

    public int statoUNITEX { get; set; }

    public List<CDGROUP_StatiDocumento> statiDocumento { get; set; }

    internal static CDGROUP_EsitiOUT FromCsv(string csvLine)
    {
        var values = csvLine.Split(';');
        var dt = values[2];
        string dtDDT = "";
        if (dt.Contains("/"))
        {
            var gg = dt.Split('/');
            dtDDT = gg[2] + gg[1] + gg[0];
        }
        else
        {
            dtDDT = dt;
        }

        CDGROUP_EsitiOUT esitiOUT = new CDGROUP_EsitiOUT()
        {
            MANDANTE = values[0],
            NUMERO_BOLLA = values[1],
            DATA_BOLLA = dtDDT
        };
        return esitiOUT;
    }
}

public class CDGROUP_StatiDocumento
{
    public int IdUnitex { get; set; }
    public string CodiceStato { get; set; }

    public static CDGROUP_StatiDocumento FromCsv(string csvLine)
    {
        var values = csvLine.Split(';');
        CDGROUP_StatiDocumento stato = new CDGROUP_StatiDocumento();
        stato.IdUnitex = Convert.ToInt32(values[0]);
        stato.CodiceStato = Convert.ToString(values[1]);
        return stato;

    }
}

public class CDGROUP_Mandante
{
    public string CodiceMandante { get; set; }
    public string RagioneSocialeMandante { get; set; }

    public static CDGROUP_Mandante FromCsv(string csvLine)
    {
        var values = csvLine.Split(';');
        CDGROUP_Mandante mandante = new CDGROUP_Mandante();
        mandante.CodiceMandante = Convert.ToString(values[0]);
        mandante.RagioneSocialeMandante = Convert.ToString(values[2]);
        return mandante;

    }
}
