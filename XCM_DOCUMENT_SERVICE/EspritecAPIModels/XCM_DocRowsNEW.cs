
using System;

public class RootobjectXCMRowsNEW
{
    public ResultXCMRowsNEW result { get; set; }
    public RowXCMRowsNew[] rows { get; set; }
}

public class ResultXCMRowsNEW
{
    public object[] messages { get; set; }
    public string info { get; set; }
    public int maxPages { get; set; }
    public bool status { get; set; }
}

public class RowXCMRowsNew
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
    public decimal um2Ratio { get; set; }
    public double qty { get; set; }
    public decimal qtyUm2 { get; set; }
    public decimal boxes { get; set; }
    public decimal packs { get; set; }
    public decimal qtyCovered { get; set; }
    public decimal qtyPlanned { get; set; }
    public decimal qtyExecuted { get; set; }
    public decimal qtyReceived { get; set; }
    public decimal qtyChecked { get; set; }
    public decimal unitCostPrice { get; set; }
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

    public override string ToString()
    {
        return $"{partNumber} - {batchNo} - {qty} - {logWareID}";
    }
}
