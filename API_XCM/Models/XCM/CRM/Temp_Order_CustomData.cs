//------------------------------------------------------------------------------
// <auto-generated>
//     Codice generato da un modello.
//
//     Le modifiche manuali a questo file potrebbero causare un comportamento imprevisto dell'applicazione.
//     Se il codice viene rigenerato, le modifiche manuali al file verranno sovrascritte.
// </auto-generated>
//------------------------------------------------------------------------------

namespace API_XCM.Models.XCM.CRM
{
    using System;
    using System.Collections.Generic;
    
    public partial class Temp_Order_CustomData
    {
        public long CustomData_Id { get; set; }
        public string CustomData_CodiceFiscale { get; set; }
        public string CustomData_CodiceIPA { get; set; }
        public string CustomData_CodiceFPR { get; set; }
        public Nullable<bool> CustomData_EntePubblico { get; set; }
        public string CustomData_TipoDitta { get; set; }
        public string CustomData_cd_PG { get; set; }
        public Nullable<long> FK_CustomData_Order_ID { get; set; }
    }
}
