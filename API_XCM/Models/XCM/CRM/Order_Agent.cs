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
    
    public partial class Order_Agent
    {
        public long OrderAgent_ID { get; set; }
        public long FK_OrderAgent_OrdersID { get; set; }
        public string FK_OrderAgent_AgentID { get; set; }
    
        public virtual Orders Orders { get; set; }
        public virtual User User { get; set; }
    }
}
