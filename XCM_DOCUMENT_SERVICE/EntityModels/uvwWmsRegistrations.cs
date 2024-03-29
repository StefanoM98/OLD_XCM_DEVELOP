//------------------------------------------------------------------------------
// <auto-generated>
//     Codice generato da un modello.
//
//     Le modifiche manuali a questo file potrebbero causare un comportamento imprevisto dell'applicazione.
//     Se il codice viene rigenerato, le modifiche manuali al file verranno sovrascritte.
// </auto-generated>
//------------------------------------------------------------------------------

namespace XCM_DOCUMENT_SERVICE.EntityModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class uvwWmsRegistrations
    {
        public int uniq { get; set; }
        public Nullable<int> GroupRegId { get; set; }
        public string SiteID { get; set; }
        public Nullable<int> WareID { get; set; }
        public Nullable<System.DateTime> DateReg { get; set; }
        public string RegTypeID { get; set; }
        public Nullable<int> RegType { get; set; }
        public Nullable<int> UniqDoc { get; set; }
        public Nullable<int> UniqDocRow { get; set; }
        public Nullable<int> StatusID { get; set; }
        public string StatusDes { get; set; }
        public string StatusColor { get; set; }
        public string RegTypeDes { get; set; }
        public string Reference { get; set; }
        public Nullable<System.DateTime> DateRef { get; set; }
        public string PrdCod { get; set; }
        public string PrdDes { get; set; }
        public Nullable<byte> PrdKind { get; set; }
        public string PrdKindDes { get; set; }
        public decimal GroupedQty { get; set; }
        public Nullable<decimal> Qty { get; set; }
        public Nullable<decimal> BoxQty { get; set; }
        public Nullable<decimal> PackQty { get; set; }
        public Nullable<decimal> BoxNo { get; set; }
        public Nullable<decimal> PackNo { get; set; }
        public string BatchNo { get; set; }
        public Nullable<System.DateTime> DateExpire { get; set; }
        public Nullable<System.DateTime> DateProd { get; set; }
        public string Barcode { get; set; }
        public string BarcodeExt { get; set; }
        public string BarcodeMaster { get; set; }
        public Nullable<decimal> NetWeight { get; set; }
        public Nullable<decimal> GrossWeight { get; set; }
        public Nullable<decimal> TotalNetWeight { get; set; }
        public Nullable<decimal> TotalGrossWeight { get; set; }
        public Nullable<decimal> TotalCube { get; set; }
        public Nullable<decimal> VarWeight { get; set; }
        public Nullable<decimal> VarWeightUnitPrice { get; set; }
        public Nullable<int> FromLocID { get; set; }
        public string FromLocDes { get; set; }
        public Nullable<int> ToLocID { get; set; }
        public string ToLocDes { get; set; }
        public string LogicWareID { get; set; }
        public string CustomerID { get; set; }
        public string CustomerDes { get; set; }
        public string AnaID { get; set; }
        public string AnaDes { get; set; }
        public string OwnerID { get; set; }
        public string OwnerDes { get; set; }
        public Nullable<int> SenderID { get; set; }
        public string SenderName { get; set; }
        public string SenderAddress { get; set; }
        public string SenderLocation { get; set; }
        public string SenderZipCode { get; set; }
        public string SenderDistrict { get; set; }
        public string SenderRegion { get; set; }
        public string SenderCountry { get; set; }
        public Nullable<int> ConsigneeID { get; set; }
        public string ConsigneeName { get; set; }
        public string ConsigneeAddress { get; set; }
        public string ConsigneeLocation { get; set; }
        public string ConsigneeZipCode { get; set; }
        public string ConsigneeDistrict { get; set; }
        public string ConsigneeRegion { get; set; }
        public string ConsigneeCountry { get; set; }
        public string Project { get; set; }
        public string SubProject { get; set; }
        public string ItemNote { get; set; }
        public string ItemInfo1 { get; set; }
        public string ItemInfo2 { get; set; }
        public string ItemInfo3 { get; set; }
        public string ItemInfo4 { get; set; }
        public string ItemInfo5 { get; set; }
        public string Variant1 { get; set; }
        public string Variant2 { get; set; }
        public string Variant3 { get; set; }
        public string Variant4 { get; set; }
        public string Variant5 { get; set; }
        public Nullable<System.DateTime> RecCreate { get; set; }
        public Nullable<int> RecUserID { get; set; }
        public Nullable<System.DateTime> RecChange { get; set; }
        public Nullable<int> RecChangeUserID { get; set; }
        public string ExternalID { get; set; }
        public string ExternalRef { get; set; }
        public string DocNum { get; set; }
        public string DocReference { get; set; }
        public string DocReference2 { get; set; }
        public string MissionTypeID { get; set; }
        public string MissionTypeDes { get; set; }
        public string MissionOpID { get; set; }
        public string MissionOpDes { get; set; }
        public string DelNum { get; set; }
        public string PDTCod { get; set; }
        public string PDTDes { get; set; }
        public Nullable<System.DateTime> QuarantineExpire { get; set; }
    }
}
