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
    
    public partial class GnXcmEntities : DbContext
    {
        public GnXcmEntities()
            : base("name=GnXcmEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<uvwWmsDocumentRows_XCM> uvwWmsDocumentRows_XCM { get; set; }
        public virtual DbSet<uvwWmsRegistrations> uvwWmsRegistrations { get; set; }
        public virtual DbSet<TmsShipmentMovGoods> TmsShipmentMovGoods { get; set; }
        public virtual DbSet<uvwGrdTmsShipmentMovGoods_Detail> uvwGrdTmsShipmentMovGoods_Detail { get; set; }
        public virtual DbSet<uvwTmsShipment> uvwTmsShipment { get; set; }
        public virtual DbSet<uvwWmsDocument> uvwWmsDocument { get; set; }
        public virtual DbSet<uvwWmsWarehouse> uvwWmsWarehouse { get; set; }
    }
}
