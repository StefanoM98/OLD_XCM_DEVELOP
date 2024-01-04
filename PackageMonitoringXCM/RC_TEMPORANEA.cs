namespace PackageMonitoringXCM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RC_TEMPORANEA
    {
        [Key]
        public long Id { get; set; }

        public long ID_DOCUMENTO { get; set; }

        [Required]
        [StringLength(50)]
        public string DESCRIZIONE_CONTENITORE { get; set; }

        public int TIPO_CONTENITORE { get; set; }

        public int QUANTITA_CONTENITORE { get; set; }

        public decimal? LARGHEZZA_PALLET { get; set; }

        public decimal? ALTEZZA_PALLET { get; set; }

        public decimal? PROFONDITA_PALLET { get; set; }

        public decimal? PESO_PALLET { get; set; }

        public override string ToString()
        {
            return $"{ID_DOCUMENTO}|{DESCRIZIONE_CONTENITORE}|{QUANTITA_CONTENITORE}";
        }
    }
}
