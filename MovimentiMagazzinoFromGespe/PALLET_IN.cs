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
    
    public partial class PALLET_IN
    {
        public long ID_REGISTRAZIONE_PALLET { get; set; }
        public string FK_ID_ANAGRAFICA_CLIENTE { get; set; }
        public long FK_ID_OPERATORE { get; set; }
        public int PALLET_IN1 { get; set; }
        public System.DateTime DATA_INSERIMENTO { get; set; }
    
        public virtual ANAGRAFICA_CLIENTI ANAGRAFICA_CLIENTI { get; set; }
        public virtual ANAGRAFICA_OPERATORI ANAGRAFICA_OPERATORI { get; set; }
    }
}
