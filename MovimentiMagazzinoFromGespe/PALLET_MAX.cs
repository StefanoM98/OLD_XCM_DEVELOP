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
    
    public partial class PALLET_MAX
    {
        public long ID_PALLET_MAX { get; set; }
        public long FK_CLIENTE { get; set; }
        public Nullable<int> PALLET_PUNTA_MAX { get; set; }
        public Nullable<System.DateTime> DATA_PALLET_PUNTA_MAX { get; set; }
        public int PALLET_START { get; set; }
        public System.DateTime DATA_PALLET_START { get; set; }
        public string MAGAZZINO { get; set; }
    }
}
