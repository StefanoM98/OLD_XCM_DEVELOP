
using System;
namespace OLDCODE
{
    public class _RootobjectXCMRows
    {
        public _ResultXCMRows result { get; set; }
        public _RowXCMRows[] rows { get; set; }


    }

    public class _ResultXCMRows
    {
        public object[] messages { get; set; }
        public bool status { get; set; }
        public string info { get; set; }
        public int maxPages { get; set; }
    }

    public class _RowXCMRows
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
        public decimal um2Ratio { get; set; }//int
        public double qty { get; set; }//int
        public decimal qtyUm2 { get; set; }//int
        public float boxes { get; set; }
        public decimal packs { get; set; }//int
        public decimal qtyCovered { get; set; }//int
        public decimal qtyPlanned { get; set; }//int
        public decimal qtyExecuted { get; set; }//int
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
            return $"{partNumber} - {batchNo} - {qty}";
        }
    }
}
