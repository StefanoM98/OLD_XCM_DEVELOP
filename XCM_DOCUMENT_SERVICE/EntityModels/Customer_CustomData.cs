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
    
    public partial class Customer_CustomData
    {
        public long CustomData_Id { get; set; }
        public string CustomData_CodiceFiscale { get; set; }
        public string CustomData_CodiceIPA { get; set; }
        public string CustomData_CodideFPR { get; set; }
        public Nullable<bool> CustomData_EntePubblico { get; set; }
        public string CustomData_TipoDitta { get; set; }
        public string CustomData_cd_PG { get; set; }
        public string FK_CustomData_Customer_ID { get; set; }
        public Nullable<long> FK_CustomData_Orders_ID { get; set; }
    
        public virtual Customer Customer { get; set; }
        public virtual Orders Orders { get; set; }
    }
}
