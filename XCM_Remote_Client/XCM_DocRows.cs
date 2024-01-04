
using System;

public class RootobjectDocumentRows
{
    public ResultDocumentRows result { get; set; }
    public DocumentRow[] rows { get; set; }
}

public class ResultDocumentRows
{
    public object[] messages { get; set; }
    public string info { get; set; }
    public int maxPages { get; set; }
    public bool status { get; set; }
}

public class DocumentRow
{
    public int id { get; set; }
    public string externalID { get; set; }
    public string partNumber { get; set; }
    public string partNumberDes { get; set; }
    public string logWareID { get; set; }
    public string batchNo { get; set; }
    public DateTime? expireDate { get; set; }
    public string um { get; set; }
    public string um2 { get; set; }
    public double um2Ratio { get; set; }
    public decimal qty { get; set; }
    public double qtyUm2 { get; set; }
    public float boxes { get; set; }
    public double packs { get; set; }
    public double qtyCovered { get; set; }
    public double qtyPlanned { get; set; }
    public double qtyExecuted { get; set; }
    public double qtyReceived { get; set; }
    public double qtyChecked { get; set; }
    public double unitCostPrice { get; set; }
    public decimal costPrice { get; set; }
    public decimal unitSellPrice { get; set; }
    public decimal sellPrice { get; set; }
    public decimal discount { get; set; }
    public decimal netSellPrice { get; set; }
    public string barcode { get; set; }
    public string barcodeExt { get; set; }
    public string barcodeMaster { get; set; }
    public string linkedID { get; set; }
    public string linkedExternalID { get; set; }
    public string info1 { get; set; }
    public string info2 { get; set; }
    public string info3 { get; set; }
    public string info4 { get; set; }
    public string info5 { get; set; }
    public string info6 { get; set; }
    public string info7 { get; set; }
    public string info8 { get; set; }
    public string info9 { get; set; }
}
