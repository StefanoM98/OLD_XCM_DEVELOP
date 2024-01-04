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
    
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.AgentAuthorization = new HashSet<AgentAuthorization>();
            this.G_ListiniAgenti = new HashSet<G_ListiniAgenti>();
            this.Order_Agent = new HashSet<Order_Agent>();
        }
    
        public string User_ID { get; set; }
        public string User_Username { get; set; }
        public string User_Email { get; set; }
        public string User_HashPassword { get; set; }
        public string User_Salt { get; set; }
        public string User_FirstName { get; set; }
        public string User_Surname { get; set; }
        public System.DateTime User_CreationDate { get; set; }
        public System.DateTime User_LastModifiedDate { get; set; }
        public Nullable<bool> User_Active { get; set; }
        public bool User_PasswordRecovered { get; set; }
        public string User_TempPassword { get; set; }
        public Nullable<System.DateTime> User_ExpireLinkRecoverPassword { get; set; }
        public long FK_User_Role_ID { get; set; }
        public string FK_User_Customer_ID { get; set; }
    
        public virtual Customer Customer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AgentAuthorization> AgentAuthorization { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<G_ListiniAgenti> G_ListiniAgenti { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_Agent> Order_Agent { get; set; }
        public virtual Role Role { get; set; }
    }
}
