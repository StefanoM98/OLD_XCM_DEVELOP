//------------------------------------------------------------------------------
// <auto-generated>
//     Codice generato da un modello.
//
//     Le modifiche manuali a questo file potrebbero causare un comportamento imprevisto dell'applicazione.
//     Se il codice viene rigenerato, le modifiche manuali al file verranno sovrascritte.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VIVISOL_DOCUMENT_SERVICE.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class TRASMISSIONE
    {
        public long ID_TRASMISSIONE { get; set; }
        public System.DateTime DATA_EVENTO { get; set; }
        public int STATO_EVENTO { get; set; }
        public string PAYLOAD { get; set; }
        public System.DateTime DATA_ULTIMO_TENTATIVO { get; set; }
        public string MESSAGGIO_ERRORE { get; set; }
        public int ID_DOCUMENTO_GESPE { get; set; }
        public int STATO_DOCUMENTO_GESPE { get; set; }
        public int FLUSSO { get; set; }
        public string GESPE_DOCNUM { get; set; }
    }
}
