﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ContenitoriXCMEntities : DbContext
    {
        public ContenitoriXCMEntities()
            : base("name=ContenitoriXCMEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ANAGRAFICA_CONTENITORI> ANAGRAFICA_CONTENITORI { get; set; }
        public virtual DbSet<REGISTRAZIONE_CONTENITORE> REGISTRAZIONE_CONTENITORE { get; set; }
    }
}