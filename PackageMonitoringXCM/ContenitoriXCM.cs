using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace PackageMonitoringXCM
{
    public partial class ContenitoriXCM : DbContext
    {
        public ContenitoriXCM()
            : base("name=ContenitoriXCM")
        {
        }

        public virtual DbSet<ANAGRAFICA_CONTENITORI> ANAGRAFICA_CONTENITORI { get; set; }
        public virtual DbSet<PALLET> PALLET { get; set; }
        public virtual DbSet<RC_TEMPORANEA> RC_TEMPORANEA { get; set; }
        public virtual DbSet<REGISTRAZIONE_CONTENITORE> REGISTRAZIONE_CONTENITORE { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ANAGRAFICA_CONTENITORI>()
                .HasMany(e => e.REGISTRAZIONE_CONTENITORE)
                .WithRequired(e => e.ANAGRAFICA_CONTENITORI)
                .HasForeignKey(e => e.ANAGRAFICA_CONTENITORE)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PALLET>()
                .Property(e => e.LARGHEZZA)
                .HasPrecision(18, 0);

            modelBuilder.Entity<PALLET>()
                .Property(e => e.ALTEZZA)
                .HasPrecision(18, 0);

            modelBuilder.Entity<PALLET>()
                .Property(e => e.PROFONDITA)
                .HasPrecision(18, 0);

            modelBuilder.Entity<PALLET>()
                .Property(e => e.PESO)
                .HasPrecision(18, 0);

            modelBuilder.Entity<PALLET>()
                .HasMany(e => e.REGISTRAZIONE_CONTENITORE)
                .WithRequired(e => e.PALLET)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RC_TEMPORANEA>()
                .Property(e => e.DESCRIZIONE_CONTENITORE)
                .IsUnicode(false);

            modelBuilder.Entity<RC_TEMPORANEA>()
                .Property(e => e.LARGHEZZA_PALLET)
                .HasPrecision(18, 0);

            modelBuilder.Entity<RC_TEMPORANEA>()
                .Property(e => e.ALTEZZA_PALLET)
                .HasPrecision(18, 0);

            modelBuilder.Entity<RC_TEMPORANEA>()
                .Property(e => e.PROFONDITA_PALLET)
                .HasPrecision(18, 0);

            modelBuilder.Entity<RC_TEMPORANEA>()
                .Property(e => e.PESO_PALLET)
                .HasPrecision(18, 0);
        }
    }
}
