namespace PackageMonitoringXCM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ANAGRAFICA_CONTENITORI
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ANAGRAFICA_CONTENITORI()
        {
            REGISTRAZIONE_CONTENITORE = new HashSet<REGISTRAZIONE_CONTENITORE>();
        }

        [Key]
        public long ID_ANAGRAFICA_CONTENITORE { get; set; }

        public int TIPO_CONTENITORE { get; set; }

        [Required]
        [StringLength(50)]
        public string DESCRIZIONE_CONTENITORE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<REGISTRAZIONE_CONTENITORE> REGISTRAZIONE_CONTENITORE { get; set; }
    }
}
