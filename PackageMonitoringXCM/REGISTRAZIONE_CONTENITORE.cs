namespace PackageMonitoringXCM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class REGISTRAZIONE_CONTENITORE
    {
        [Key]
        public long ID_REGISTRAZIONE_CONTENITORE { get; set; }

        public long ID_DOCUMENTO { get; set; }

        public long ANAGRAFICA_CONTENITORE { get; set; }

        public int QUANTITA_CONTENITORE { get; set; }

        public long ID_PALLET { get; set; }

        public virtual ANAGRAFICA_CONTENITORI ANAGRAFICA_CONTENITORI { get; set; }

        public virtual PALLET PALLET { get; set; }
    }
}
