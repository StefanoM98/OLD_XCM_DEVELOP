namespace PackageMonitoringXCM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PALLET")]
    public partial class PALLET
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PALLET()
        {
            REGISTRAZIONE_CONTENITORE = new HashSet<REGISTRAZIONE_CONTENITORE>();
        }

        [Key]
        public long ID_PALLET { get; set; }

        public decimal LARGHEZZA { get; set; }

        public decimal ALTEZZA { get; set; }

        public decimal PROFONDITA { get; set; }

        public decimal PESO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<REGISTRAZIONE_CONTENITORE> REGISTRAZIONE_CONTENITORE { get; set; }
    }
}
