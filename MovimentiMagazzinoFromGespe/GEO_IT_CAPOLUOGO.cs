//------------------------------------------------------------------------------
// <auto-generated>
//     Codice generato da un modello.
//
//     Le modifiche manuali a questo file potrebbero causare un comportamento imprevisto dell'applicazione.
//     Se il codice viene rigenerato, le modifiche manuali al file verranno sovrascritte.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MovimentiMagazzinoFromGespe
{
    using System;
    using System.Collections.Generic;
    
    public partial class GEO_IT_CAPOLUOGO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GEO_IT_CAPOLUOGO()
        {
            this.TMSF_G_LISTINI = new HashSet<TMSF_G_LISTINI>();
        }
    
        public long ID_CAPOLUOGO_GEO_IT { get; set; }
        public string CAP { get; set; }
        public string CITTA { get; set; }
        public string REGIONE { get; set; }
        public string PROVINCIA { get; set; }
        public Nullable<int> ABITANTI { get; set; }
        public string CODICE_ISTAT { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TMSF_G_LISTINI> TMSF_G_LISTINI { get; set; }
    }
}