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
    
    public partial class ViewPrdCRM
    {
        public int ID { get; set; }
        public string PrdCod { get; set; }
        public string PrdDes { get; set; }
        public string CustomerID { get; set; }
        public Nullable<System.DateTime> RecChange { get; set; }
        public Nullable<System.DateTime> RecCreate { get; set; }
        public Nullable<decimal> PrzMin { get; set; }
    }
}
