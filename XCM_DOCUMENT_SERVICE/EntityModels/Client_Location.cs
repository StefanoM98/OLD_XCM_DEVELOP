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
    
    public partial class Client_Location
    {
        public long Location_Id { get; set; }
        public string Location_Description { get; set; }
        public string Location_ZipCode { get; set; }
        public string Location_Location { get; set; }
        public string Location_Address { get; set; }
        public string Location_District { get; set; }
        public string Location_Region { get; set; }
        public string Location_Country { get; set; }
        public bool Location_IsDefault { get; set; }
        public System.DateTime Location_LastModifiedDate { get; set; }
        public System.DateTime Location_CreationDate { get; set; }
        public string Location_LastModifiedUserId { get; set; }
        public Nullable<int> Location_GespeLocationId { get; set; }
        public bool Location_IsActive { get; set; }
        public string FK_Location_Client_ID { get; set; }
    
        public virtual Customer_Client Customer_Client { get; set; }
    }
}
